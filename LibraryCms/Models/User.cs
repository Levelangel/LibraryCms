using System;

namespace LibraryCms.Models
{
    public class User
    {
        public string UserID { get; set; }

        public Role Role { get; set; }

        public string Number { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Mail { get; set; }

        public string Phone { get; set; }

        public string QQ { get; set; }

        public string Sex { get; set; }

        public override string ToString()
        {
            return EncodeString();
        }

        private string EncodeString()
        {
            var guid = Guid.NewGuid();
            string info = UserID + "→" + Mail + "→" + Phone + "→" + QQ;
            string time = DateTime.Now.Ticks.ToString();
            string strGuid = guid.ToString();
            string key = strGuid.Substring(0, 8);
            string toEncode = info + "→" + time + "→" + guid.ToString();
            string final = Check.Encrypt(toEncode, key);
            final += key;
            final = final.ToUpper();
            return final;
        }

        public User(string info)
        {
            try
            {
                string final = info.ToLower();
                string key = final.Substring(final.Length - 8, 8);
                string toDecode = final.Substring(0, final.Length - 8);
                string first = Check.Decrypt(toDecode, key);
                string[] str = first.Split('→');
                string strGuid = str[5];
                string time = str[4];
                if (strGuid.Substring(0, 8) != key)
                {
                    UserID = "Incorrect Data";
                    return;
                }
                long timeOver = DateTime.Now.Ticks - long.Parse(time);
                DateTime nowTime = new DateTime(timeOver);
                //30分钟之内有效
                if (nowTime.Year == 1 && nowTime.Month == 1 && nowTime.Day == 1 && nowTime.Hour == 0 && nowTime.Minute < 30)
                {
                    UserID = str[0];
                    Mail = str[1];
                    Phone = str[2];
                    QQ = str[3];
                }
                else
                {
                    UserID = "Time Out";
                }
            }
            catch (Exception)
            {
                UserID = "Incorrect Data";
                return;
            }
        }
        public User()
        {
        }
    }
}