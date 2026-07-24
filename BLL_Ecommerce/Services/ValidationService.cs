using BLL_Ecommerce.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace BLL_Ecommerce.Services
{
    public class ValidationService : IValidationService
    {
        // Aggiunti i Magic Bytes per .webp ("RIFF" -> 0x52, 0x49, 0x46, 0x46)
        private static readonly Dictionary<string, List<byte[]>> KnownSignatures = new Dictionary<string, List<byte[]>>
        {
            { ".jpg",  new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF } } },
            { ".jpeg", new List<byte[]> { new byte[] { 0xFF, 0xD8, 0xFF } } },
            { ".png",  new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { ".gif",  new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
            { ".webp", new List<byte[]> { new byte[] { 0x52, 0x49, 0x46, 0x46 } } }, // Magic bytes WebP
            { ".pdf",  new List<byte[]> { new byte[] { 0x25, 0x50, 0x44, 0x46 } } },
            { ".docx", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } },
            { ".xlsx", new List<byte[]> { new byte[] { 0x50, 0x4B, 0x03, 0x04 } } }
        };

        public ValidationResult ValidateImage(byte[] fileBytes, string fileName, int maxKb = 2048, string[] allowedExtensions = null)
        {
            // Se non vengono passate estensioni, usiamo un fallback standard
            if (allowedExtensions == null || allowedExtensions.Length == 0)
            {
                allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            }

            // 1. Controllo Peso, Estensione e Magic Bytes
            var basicCheck = ValidateBasicFile(fileBytes, fileName, allowedExtensions, maxKb);
            if (!basicCheck.IsValid) return basicCheck;

            // 2. Controllo Integrità visiva (GDI+)
            string ext = Path.GetExtension(fileName)?.ToLower();

            // Nota: System.Drawing (GDI+) su .NET 4.8.1 valida nativamente JPG, PNG, GIF.
            // Per WEBP usiamo la verifica rigorosa dei Magic Bytes fatta al punto 1.
            if (ext != ".webp")
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream(fileBytes))
                    using (Image img = Image.FromStream(ms))
                    {
                        if (img.Width <= 0 || img.Height <= 0)
                            return ValidationResult.Fail("L'immagine ha dimensioni non valide.");
                    }
                }
                catch
                {
                    return ValidationResult.Fail("Il file è un'immagine corrotta o non leggibile.");
                }
            }

            return ValidationResult.Success();
        }

        public ValidationResult ValidateDocument(byte[] fileBytes, string fileName, int maxKb = 20000, string[] allowedExtensions = null)
        {
            if (allowedExtensions == null || allowedExtensions.Length == 0)
            {
                allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
            }
            return ValidateBasicFile(fileBytes, fileName, allowedExtensions, maxKb);
        }

        private ValidationResult ValidateBasicFile(byte[] fileBytes, string fileName, string[] allowedExtensions, int maxKb)
        {
            if (fileBytes == null || fileBytes.Length == 0)
                return ValidationResult.Fail("Nessun file inviato o file vuoto.");

            // Calcolo peso in Byte (1 KB = 1024 Bytes)
            long maxSizeBytes = maxKb * 1024L;
            if (fileBytes.Length > maxSizeBytes)
            {
                double maxMbDisplay = maxKb / 1024.0;
                return ValidationResult.Fail($"Il file supera la dimensione massima consentita di {maxKb} KB ({maxMbDisplay:0.#} MB).");
            }

            // Controllo Estensione (Pulisce gli spazi e converte in minuscolo)
            string ext = Path.GetExtension(fileName)?.ToLower();
            var cleanAllowed = allowedExtensions.Select(e => e.Trim().ToLower()).ToArray();

            if (string.IsNullOrEmpty(ext) || !cleanAllowed.Contains(ext))
                return ValidationResult.Fail($"Estensione non valida ({ext}). Formati supportati: {string.Join(", ", cleanAllowed)}.");

            // Controllo Magic Bytes
            if (!VerifyMagicBytes(fileBytes, ext))
                return ValidationResult.Fail("Il contenuto reale del file non corrisponde alla sua estensione.");

            return ValidationResult.Success();
        }

        private bool VerifyMagicBytes(byte[] fileBytes, string extension)
        {
            if (!KnownSignatures.ContainsKey(extension)) return false;

            var signatures = KnownSignatures[extension];
            return signatures.Any(sig => fileBytes.Length >= sig.Length && fileBytes.Take(sig.Length).SequenceEqual(sig));
        }
    }
}