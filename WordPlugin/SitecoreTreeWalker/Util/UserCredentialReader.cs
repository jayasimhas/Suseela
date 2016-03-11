using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

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

        public CookieCollection GetCookie(string username)
        {
            var uName = username.Split('\\').Length > 1 ? username.Split('\\')[1] : username;
            using (var iStream = new IsolatedStorageFileStream(uName + "-" + Constants.EDITOR_ENVIRONMENT + "-" + COOKIE_FILE, FileMode.OpenOrCreate, _isoStore))
            {
                var formatter = new BinaryFormatter();   
                CookieCollection retrievedCookies = null;
                    retrievedCookies = (CookieCollection)formatter.Deserialize(iStream);

                return retrievedCookies;
            }

        }

        public void WriteCookie(CookieCollection cookie, string username)
        {
            var uName = username.Split('\\').Length > 1 ? username.Split('\\')[1] : username;
            using (var oStream = new IsolatedStorageFileStream(uName + "-" + Constants.EDITOR_ENVIRONMENT + "-" + COOKIE_FILE, FileMode.Create, _isoStore))
            {                   
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(oStream, cookie);      
               
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
