using UnityEngine;
using System.Security.Cryptography;
using System;
using System.Linq;
using System.Text;
using System.IO;

public class DatabaseProcessor : MonoBehaviour
{

    public class Decryptor
    {
        private string randomCode = "IFOZTuxX0sUd3/PO8nU9u6Z1zrX0TBMVsr3WKEDjCWL2r7dspa7fBH52oYHAIqItgaHoUCZDRW4rne8yQdmrR8BgdRm5JC4hGsXEenDS2V38UrJF281ZSpfANks0uBv+";
        private string Sqlpassword = "5gVmXKtUCxqzZ/3te7WYvuLHKJLcwtE9e8IzjomgWA5I8VQ5bjGKBRv0h/sRXqnlw2fIfCdWrTrHUn2FZRJA7QOPBOf3wmvZfaS2NkoOnN5OfYiDg5bHcklZaYNJi16E";
        private const int Keysize = 256;
        private const int DerivationIterations = 1000;

        public string getRandomCode()
        {
            return randomCode;
        }

        public string getSqlPassword()
        {
            return Sqlpassword;
        }

        public string getPassword()
        {
            string connectionPassword = Decrypt(Sqlpassword, randomCode);
            return connectionPassword;
        }

        public string Decrypt(string DecryptText, string passCode)
        {

            var decryptTextBytesWithSaltAndIv = Convert.FromBase64String(DecryptText);
            var saltStringBytes = decryptTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            var ivStringBytes = decryptTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            var decryptTextBytes = decryptTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(decryptTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            var password = new Rfc2898DeriveBytes(passCode, saltStringBytes, DerivationIterations);

            var keyBytes = password.GetBytes(Keysize / 8);
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.BlockSize = 256;
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.PKCS7;
                var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes);


                var memoryStream = new MemoryStream(decryptTextBytes);

                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                var plainTextBytes = new byte[decryptTextBytes.Length];
                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }

        }
    }

    Decryptor DecryptorObject = new Decryptor();
    SqlConnection myConnection;

    public bool Connect()
    {
        string connectionPassword = DecryptorObject.getPassword();
        string connectionString =
                 "Server=den1.mssql3.gear.host;" +
                 "Database=seniordemodb1;" +
                 "User ID=seniordemodb1;" +
                 "Password=" + connectionPassword + ";";
        try
        {
            myConnection = new SqlConnection(connectionString);
            myConnection.Open();
            return true;
        }

        catch (System.Exception e)
        {
            Debug.Log(e.Message + " ERRORDBProccessor");
            return false;
        }
    }

    public DataTable GetData(string query)
    {
        DataTable myDataTable = new DataTable();

        SqlCommand myCommand;
        myCommand = new SqlCommand(query, myConnection);
        Debug.Log(myConnection.State);
        int commandResult = myCommand.ExecuteNonQuery();

        using (SqlDataReader reader = myCommand.ExecuteReader())
        {
            myDataTable.Load(reader);
        }
        return myDataTable;
    }




}
