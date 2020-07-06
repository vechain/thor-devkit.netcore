using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Org.VeChain.Thor.Devkit.UnitTest
{
    public static class ResourcesHelper
    {
        public static string GetResourceFile(string sourcefilename,Type currentAssemble)
		{
			string sText = "";
			try
			{
				if (!string.IsNullOrEmpty (sourcefilename) && currentAssemble != null) 
				{
					Assembly oAssembly = Assembly.GetAssembly (currentAssemble);
					string[] arrCurrentResourceNames = oAssembly.GetManifestResourceNames();
					string sFullName = GetExists(arrCurrentResourceNames, sourcefilename);
					if (sFullName.Length > 0) 
					{
						using (Stream oStream = oAssembly.GetManifestResourceStream (sFullName)) 
						{
							using (StreamReader oStreamReader = new StreamReader (oStream, Encoding.UTF8)) 
							{
								oStreamReader.BaseStream.Seek(0, SeekOrigin.Begin);
								sText = oStreamReader.ReadToEnd();
							}
						}
					}
				}
			}
			catch(Exception ex) 
			{
				throw ex;
			}
			return sText;
		}

        public static byte [] GetResourceBytes (string sourcefilename, Type currentAssemble)
		{
			byte [] bytes = null;
			try 
			{
				if (!string.IsNullOrEmpty (sourcefilename) && currentAssemble != null) 
				{
					Assembly oAssembly = Assembly.GetAssembly (currentAssemble);
					string [] arrCurrentResourceNames = oAssembly.GetManifestResourceNames ();
					string sFullName = GetExists (arrCurrentResourceNames, sourcefilename);
					if (sFullName.Length > 0) 
					{
						using (Stream oStream = oAssembly.GetManifestResourceStream (sFullName)) 
						{
							bytes = new byte [oStream.Length];
							oStream.Read (bytes, 0, bytes.Length);
							oStream.Seek (0, SeekOrigin.Begin);
						}
					}
				}
			} 
			catch (Exception ex) 
			{
				throw ex;
			}
			return bytes;
		}

        private static string GetExists(string[] allResource, string fileName)
		{
			Object oLock = new Object();
			lock (oLock)
			{
				string sRet = "";
				if (allResource == null || allResource.Length == 0)
				{
					return sRet;
				}
				foreach (string name in allResource)
				{
					if (name.Equals(fileName))
					{
						sRet = fileName;
						break;
					}
				}
				return sRet;
			}
		}
    }
}