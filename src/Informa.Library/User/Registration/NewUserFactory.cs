namespace Informa.Library.User.Registration
{
	public class NewUserFactory : INewUserFactory
	{
		public INewUser Create()
		{
			return new NewUser();
		}
	}
}
