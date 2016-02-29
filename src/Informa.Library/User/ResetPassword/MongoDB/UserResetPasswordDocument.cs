using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Informa.Library.User.ResetPassword.MongoDB
{
	public class UserResetPasswordDocument : IUserResetPassword
	{
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		public string Token { get; set; }
		public string Username { get; set; }
		public DateTime Expiration { get; set; }
		public string Name { get; set; }
	}
}
