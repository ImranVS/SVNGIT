using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSWebDO
{
   public class UserSecurityQuestions
    {
       public UserSecurityQuestions()
	{}
    public int ID
    {
        get { return _ID; }
        set { _ID = value; }  
    }
    private int _ID;

    public string SecurityQuestion
    {

        get { return _SecurityQuestion; }
        set { _SecurityQuestion = value; }

    }
    private string _SecurityQuestion;

    public UserSecurityQuestions(
        int ID, string SecurityQuestion)
    {
        this._ID = ID;
        this._SecurityQuestion = SecurityQuestion;
    
    }

    }
}
