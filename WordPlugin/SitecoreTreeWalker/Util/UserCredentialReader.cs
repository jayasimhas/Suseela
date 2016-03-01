using System.IO;
using System.IO.IsolatedStorage;

namespace InformaSitecoreWord.Util
{
	class UserCredentialReader
	{
		private static UserCredentialReader _reader = new UserCredentialReader();
		private const string PASSWORD_FILE = "UsernamePassword.txt";
	    private const string COOKIE_FILE = "session.txt";
        private const string ENVIRONMENT_FILE = "EditorEnvironment.txt";
		private readonly IsolatedStorageFile _isoStore =
						IsolatedStorageFile.GetStore(
						IsolatedStorageScope.User | IsolatedStorageScope.Assembly,
						null, null);

		private UserCredentialReader()
		{
			
		}

		public static UserCredentialReader GetReader()
		{
			return _reader;
		}

		public void Clear()
		{
			var truncate = new IsolatedStorageFileStream(PASSWORD_FILE, FileMode.Truncate, _isoStore);
			truncate.Close();
		}

		public void Write(string username, string password)
		{
			using (var oStream = new IsolatedStorageFileStream(PASSWORD_FILE, FileMode.Create, _isoStore))
			{
				using (var writer = new StreamWriter(oStream))
				{
					writer.WriteLine(username);
					writer.WriteLine(password);
				}
			}
		}


		public void Write(string username)
		{
			using (var oStream = new IsolatedStorageFileStream(PASSWORD_FILE, FileMode.Create, _isoStore))
			{
				using (var writer = new StreamWriter(oStream))
				{
					writer.WriteLine(username);
				}
			}
		}

		public string GetUsername()
		{
			using (var iStream = new IsolatedStorageFileStream(PASSWORD_FILE, FileMode.OpenOrCreate, _isoStore))
			{
				using (var reader = new StreamReader(iStream))
				{
					return reader.ReadLine();
				}
			}
		}

		public string GetPassword()
		{
			using (var iStream = new IsolatedStorageFileStream(PASSWORD_FILE, FileMode.OpenOrCreate, _isoStore))
			{
				using (var reader = new StreamReader(iStream))
				{
					reader.ReadLine(); // first line is the username
					return reader.ReadLine();
				}
			}
		}

		public bool HasPassword()
		{
			using (var iStream = new IsolatedStorageFileStream(PASSWORD_FILE, FileMode.OpenOrCreate, _isoStore))
			{
				using (var reader = new StreamReader(iStream))
				{
					reader.ReadLine();
					return reader.ReadLine() != null;
				}
			}
		}

        public string GetCookie()
        {
            using (var iStream = new IsolatedStorageFileStream(COOKIE_FILE, FileMode.OpenOrCreate, _isoStore))
            {
                using (var reader = new StreamReader(iStream))
                {
                    reader.ReadLine(); // first line is the username
                    return reader.ReadLine();
                }
            }
        }

        public bool HasCookie()
        {
            using (var iStream = new IsolatedStorageFileStream(COOKIE_FILE, FileMode.OpenOrCreate, _isoStore))
            {
                using (var reader = new StreamReader(iStream))
                {
                    reader.ReadLine();
                    return reader.ReadLine() != null;
                }
            }
        }

        public void WriteCookie(string cookie)
        {
            using (var oStream = new IsolatedStorageFileStream(COOKIE_FILE, FileMode.Create, _isoStore))
            {
                using (var writer = new StreamWriter(oStream))
                {
                    writer.WriteAsync(cookie);
                }
            }
        }




        public string GetEditorEnvironment()
        {
            using (var iStream = new IsolatedStorageFileStream(ENVIRONMENT_FILE, FileMode.OpenOrCreate, _isoStore))
            {
                using (var reader = new StreamReader(iStream))
                {
                    return reader.ReadLine();
                }
            }
        }


        public string GetEditorEnvironmentForgotPasswordLink()
        {
            using (var iStream = new IsolatedStorageFileStream(ENVIRONMENT_FILE, FileMode.OpenOrCreate, _isoStore))
            {
                using (var reader = new StreamReader(iStream))
                {
                    reader.ReadLine();
                    reader.ReadLine();
                    reader.ReadLine();
                    return reader.ReadLine();
                }
            }
        }

        public string GetEditorEnvironmentLoginUrl()
        {
            using (var iStream = new IsolatedStorageFileStream(ENVIRONMENT_FILE, FileMode.OpenOrCreate, _isoStore))
            {
                using (var reader = new StreamReader(iStream))
                {
                    reader.ReadLine();
                    reader.ReadLine();
                    return reader.ReadLine();
                }
            }
        }

        public string GetEditorEnvironmentServerUrl()
        {
            using (var iStream = new IsolatedStorageFileStream(ENVIRONMENT_FILE, FileMode.OpenOrCreate, _isoStore))
            {
                using (var reader = new StreamReader(iStream))
                {
                    reader.ReadLine();
                    return reader.ReadLine();
                }
            }
        }


        public void WriteEditorEnvironment(string environment)
        {
            using (var oStream = new IsolatedStorageFileStream(ENVIRONMENT_FILE, FileMode.Create, _isoStore))
            {
                using (var writer = new StreamWriter(oStream))
                {
                    writer.WriteLine(environment);
                }
            }
        }

        public void WriteEditorEnvironment(string environment, string serverUrl, string loginUrl, string forgotPasswordLink)
        {
            using (var oStream = new IsolatedStorageFileStream(ENVIRONMENT_FILE, FileMode.Create, _isoStore))
            {
                using (var writer = new StreamWriter(oStream))
                {
                    writer.WriteLine(environment);
                    writer.WriteLine(serverUrl);
                    writer.WriteLine(loginUrl);
                    writer.WriteLine(forgotPasswordLink);
                }
            }
        }


        public bool HasEditorEnvironment()
        {
            using (var iStream = new IsolatedStorageFileStream(ENVIRONMENT_FILE, FileMode.OpenOrCreate, _isoStore))
            {
                using (var reader = new StreamReader(iStream))
                {
                    return reader.ReadLine() != null;
                }
            }
        }

        public void ClearEditorEnvironment()
        {
            var truncate = new IsolatedStorageFileStream(ENVIRONMENT_FILE, FileMode.Truncate, _isoStore);
            truncate.Close();
        }
    }
}
