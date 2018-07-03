﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using RsaSecureToken;

namespace RsaSecureToken.Tests
{
    [TestClass()]
    public class AuthenticationServiceTests
    {
        [TestMethod()]
        public void IsValidTest_只有驗證Authentication合法或非法()
        {
            //arrange
            IProfile profile = Substitute.For<IProfile>();
            profile.GetPassword("Joey").Returns("91");

            IToken token = Substitute.For<IToken>();
            token.GetRandom("Joey").Returns("abc");

            ILog log = Substitute.For<ILog>();
            AuthenticationService target = new AuthenticationService(profile, token, log);
            string account = "Joey";
            string password = "wrong password";
            // 正確的 password 應為 "91abc"

            //act
            bool actual;
            actual = target.IsValid(account, password);

            // assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void IsValidTest_如何驗證當非法登入時有正確紀錄log()
        {
            // 試著使用 stub object 的 ReturnsForAnyArgs() 方法
            //例如：profile.GetPassword("").ReturnsForAnyArgs("91"); // 不管GetPassword()傳入任何參數，都要回傳 "91"

            // step 1: arrange, 建立 mock object
            // ILog log = Substitute.For<ILog>();

            // step 2: act

            // step 3: assert, mock object 是否有正確互動
            //log.Received(1).Save("account:Joey try to login failed"); //Received(1) 可以簡化成 Received()
            var profile = Substitute.For<IProfile>();
            profile.GetPassword("Joey").Returns("91");
            var token = Substitute.For<IToken>();
            token.GetRandom("Joey").Returns("abc");
            var mockLog = Substitute.For<ILog>();
            var authenticationService = new AuthenticationService(profile, token, mockLog);
            authenticationService.IsValid("Joey", "wrong passcode");
            //用完整的字串會造成測試的不穩定
            //mockLog.Received(1).Save("account:Joey try to login failed");
            //部份字串比對
            //mockLog.Received(1).Save(Arg.Is<string>(s => s.Contains("Joey") && s.Contains("login failed")));
            //只驗證有呼叫log，不管log的內容
            mockLog.ReceivedWithAnyArgs(1).Save("");
            //Assert.Inconclusive();
        }

        [TestMethod]
        public void is_valid_should_not_log()
        {
            //arrange
            IProfile stubProfile = Substitute.For<IProfile>();
            stubProfile.GetPassword("Joey").Returns("91");

            IToken stubToken = Substitute.For<IToken>();
            stubToken.GetRandom("Joey").Returns("abc");

            ILog mockLog = Substitute.For<ILog>();
            AuthenticationService target = new AuthenticationService(stubProfile, stubToken, mockLog);
            string account = "Joey";
            string password = "91abc";

            //act
            target.IsValid(account, password);
            //asert
            mockLog.DidNotReceiveWithAnyArgs().Save("");
            //mockLog.Received(1).Save(Arg.Is<string>(m => m.Contains("Joey") && m.Contains("login failed")));
            //mockLog.Received(1).Save("account:Joey try to login failed");
        }
    }
}