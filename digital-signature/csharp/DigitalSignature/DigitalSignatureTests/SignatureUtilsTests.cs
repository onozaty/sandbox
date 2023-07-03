using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalSignature
{
    [TestClass()]
    public class SignatureUtilsTests
    {
        [TestMethod()]
        public void Sign()
        {
            string keyDirPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "../../../../../../"));
            string privateKeyPath = Path.Combine(keyDirPath, "private-pkcs8.key");
            string publicKeyPath = Path.Combine(keyDirPath, "public.key");

            string target = "Hello";

            string signature = SignatureUtils.Sign(target, privateKeyPath);

            Assert.AreEqual(signature, "CPzRkZ82cvMucv6Ph+Eodg1x+afwQF7oL22oMJ1ZrHoHHqusdhN2YzJ+8sTaZmLCPCtadkEEKr/bbI07rHTof4OtD2FGHOYHteng4a8tHZiwuBJLjpDGCPZAlCnl/jpiDA54mR9WZLv4tgAbMPTg9vMqHQVYMJaJAl6e/1RRykunKXAF67iREab6SCxL1Gfjjfax9KHH6GEZnfkjEZ02t0bvI7Qo9cgKAYWe87tOsWYb+RR2gjNHsktp45QG+4eTE17RXnPrY3OY2dBifz1zwDqqEIg1DPrRbSsh04gJXyaB4/MttDOzYJdHVejlacs7iqwRmsysvcMYtVsHeZqIvg==");

            bool verifed = SignatureUtils.Verify(target, publicKeyPath, signature);

            Assert.IsTrue(verifed);
        }
    }
}