using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.DataAnnotations;

namespace GeoSearch
{
    public class RetrieveNumber
    {
        private int _totalNumber;
        private int _firstNumber;
        private int _regularNumber;

        public const int maximum_totalNumber = 10000;
        public const int minimum_totalNumber = 50;

        public const int maximum_firstNumber = 5000;
        public const int minimum_firstNumber = 1;

        public const int maximum_regularNumber = 5000;
        public const int minimum_regularNumber = 1;


        //[Range(100, 10000, ErrorMessage = "Please input a positive integer between 100 and 10000")]
        public int TotalNumber
        {
            get { return _totalNumber; }
            set
            {
                //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "TotalNumber" });
                if (value > maximum_totalNumber || value < minimum_totalNumber)
                {
                    throw new Exception("Please input a positive integer between " + minimum_totalNumber + " and " + maximum_totalNumber);
                }
                _totalNumber = value;
            }
        }

        
        //[Range(1, 10000, ErrorMessage = "Please input a positive integer between 1 and 10000")]
        public int firstNumber
        {
            get { return _firstNumber; }
            set
            {
                //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "firstNumber" });
                if (value > maximum_firstNumber || value < minimum_firstNumber)
                {
                    throw new Exception("Please input a positive integer between " + minimum_firstNumber + " and " + maximum_firstNumber);
                }
                _firstNumber = value;
            }
        }

        
        //[Range(1, 10000, ErrorMessage = "Please input a positive integer between 1 and 10000")]
        public int regularNumber
        {
            get { return _regularNumber; }
            set
            {
                //Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "regularNumber" });
                if (value > maximum_regularNumber || value < minimum_regularNumber)
                {
                    throw new Exception("Please input a positive integer between " + minimum_regularNumber + " and " + maximum_regularNumber);
                }
                _regularNumber = value;
            }
        }
        
        //private string _name;
        //[CustomizeValidation]
        //public string Name
        //{
        //    get { return _name; }
        //    set 
        //    {
        //        Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "Name" });
        //        if (string.IsNullOrEmpty(value))
        //        {
        //            throw new Exception("用户名不能为空.");
        //        }
        //        _name = value; 
        //    }
        //}

        //private string _password;
        //[StringLength(6, ErrorMessage="密码不能超过6个字符")]
        //public string password
        //{
        //    get { return _password; }
        //    set 
        //    {
        //        Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "password" });
        //        _password = value; 
        //    }
        //}

        //private string _email;
        //[Required(ErrorMessage = "必填选项")]
        //[RegularExpression(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$",ErrorMessage="请输入正确的Email格式")]
        //public string email
        //{
        //    get { return _email; }
        //    set 
        //    {
        //        var tmpValidator = new ValidationContext(this, null, null);
        //        tmpValidator.MemberName = "email";
        //        Validator.ValidateProperty(value, tmpValidator);
        //        _email = value; 
        //    }
        //}

        public RetrieveNumber()
        {
        }  
    }
}
