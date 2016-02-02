﻿using System;
using System.Collections.Generic;
using System.Web.Security;
using Informa.Web.Areas.Account.Models;   
using Informa.Models.FactoryInterface;
using Sitecore.Security.Authentication;

namespace Informa.Web.Controllers
{
	public static class SitecoreUtil
	{
		//TODO: Business logic for Article Number Generation
		/// <summary>
		/// This method Generates the Article Number
		/// </summary>
		/// <param name="article"></param>
		/// <param name="publication"></param>
		/// <param name="articleDate"></param>
		/// <returns></returns>
		public static string GetNextArticleNumber(string article, Guid publication, DateTime articleDate)
		{
			string yymmdd = $"{articleDate:yyMMdd}";
			string prefix = GetPublicationPrefix(publication) + yymmdd;
			string number = prefix + article;
			return number;
		}

		/// <summary>
		/// This method gets the Publication Prefix which is used in Article Number Generation.
		/// </summary>
		/// <param name="publicationGuid"></param>
		/// <returns></returns>
		public static string GetPublicationPrefix(Guid publicationGuid)
		{
			var publicationPrefixDictionary =
				new Dictionary<Guid, string>
					{
						{new Guid("{3818C47E-4B75-4305-8F01-AB994150A1B0}"), "SC"},
					};

			string value;
			return publicationPrefixDictionary.TryGetValue(publicationGuid, out value) ? value : null;
		}

		public static WordPluginModel.UserStatusStruct GetUserStatus(string username, string password)
		{
			var userStatus = new WordPluginModel.UserStatusStruct { UserName = username };

			MembershipUser user = Membership.GetUser(username);
			if (user == null)
			{
				var domainAndUser = Sitecore.Context.Domain + "\\" + username;
				user = Membership.GetUser(domainAndUser);
				if (user == null)
				{
					userStatus.LoginSuccessful = false;
					return userStatus;
				}
			}

			userStatus.LoginAttemptsRemaining = 1;
			//TODO
			//userStatus.LoginAttemptsRemaining = AttemptedPasswordAttemptsRemaining(user);
			userStatus.LockedOut = user.IsLockedOut;
			bool wasUserLockedOut = user.IsLockedOut;

			try
			{
				userStatus.LoginSuccessful = AuthenticationManager.Login(username, password);
			}
			catch (Exception e)
			{ //regardless of exception, if the above code fails, the user is not authenticated
				userStatus.LoginSuccessful = false;
			}

			user = Membership.GetUser(user.UserName);
			if (user != null)
			{
				// TODO - if the user became locked out because of this attempt, send an email to the admins.
				if (user.IsLockedOut && !wasUserLockedOut)
				{
					//_lockedOutUserEmailer.UserBecameLockedOut(user);
				}
			}

			userStatus.LoginAttemptsRemaining = 1;
			//TODO
			//userStatus.LoginAttemptsRemaining = AttemptedPasswordAttemptsRemaining(user);
			userStatus.LockedOut = user.IsLockedOut;

			return userStatus;
		}

		//TODO _ add a check to get the attempts left
		/*
		public static int AttemptedPasswordAttemptsRemaining(MembershipUser user)
		{
			var membershipProvider = (CustomSqlMembershipProvider)Membership.Providers["sql"];
			if (membershipProvider == null)
			{
				return 0;
			}

			return membershipProvider.GetRemainingPasswordAttempts((Guid)user.ProviderUserKey);
		}
		*/
	}
}