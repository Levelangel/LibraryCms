using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryCms.Models
{
    public class Question
    {
        public string QuestionId { get; set; }
        public string Content { get; set; }
        public string answerA { get; set; }
        public string answerB { get; set; }
        public string answerC { get; set; }
        public string answerD { get; set; }
        public string answerE { get; set; }
        public string Correct { get; set; }//"00000"
        public int Type { get; set; }//0:单选 1:多选 2:判断
    }
}