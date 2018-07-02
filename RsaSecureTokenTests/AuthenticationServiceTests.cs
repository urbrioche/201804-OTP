using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RsaSecureToken;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace RsaSecureToken.Tests
{
    [TestClass()]
    public class AuthenticationServiceTests
    {
        [TestMethod()]
        public void IsValidTest()
        {
            var profileDao = Substitute.For<IProfileDao>();
            var rsaTokenDao = Substitute.For<IRsaTokenDao>();
            profileDao.GetPassword("joey").Returns("91");
            rsaTokenDao.GetRandom("joey").Returns("000000");
            var target = new AuthenticationService(profileDao, rsaTokenDao);

            var actual = target.IsValid("joey", "91000000");

            //always failed
            Assert.IsTrue(actual);
        }
    }
}