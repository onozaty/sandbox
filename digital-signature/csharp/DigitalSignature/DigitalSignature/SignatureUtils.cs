using System.Security.Cryptography;
using System.Text;

namespace DigitalSignature
{
    public class SignatureUtils
    {
        public static string Sign(string target, string privateKeyPath)
        {
            // PKCS#8 形式
            string privateKeyPEM = string.Join(
                "",
                File.ReadAllLines(privateKeyPath, Encoding.UTF8)
                    .Where(x => x != "-----BEGIN PRIVATE KEY-----" && x != "-----END PRIVATE KEY-----"));

            byte[] privateKeyBytes = Convert.FromBase64String(privateKeyPEM);

            using (var rsa = RSA.Create())
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");

                byte[] signature = rsaFormatter.CreateSignature(HashData(target));

                return Convert.ToBase64String(signature);
            }
        }
        public static bool Verify(string target, string publicKeyPath, string signature)
        {
            string publicKeyPEM = string.Join(
                "",
                File.ReadAllLines(publicKeyPath, Encoding.UTF8)
                    .Where(x => x != "-----BEGIN PUBLIC KEY-----" && x != "-----END PUBLIC KEY-----"));

            byte[] publicKeyBytes = Convert.FromBase64String(publicKeyPEM);

            using (var rsa = RSA.Create())
            {
                rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");

                return rsaDeformatter.VerifySignature(HashData(target), Convert.FromBase64String(signature));
            }
        }

        private static byte[] HashData(string target)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(target));
            }
        }
    }
}
