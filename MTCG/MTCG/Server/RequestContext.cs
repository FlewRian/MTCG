using System;
using System.Collections.Generic;
using System.Text;

namespace MTCG
{
    public class RequestContext
    {
        public string HttpVerb { get; set; }
        public string FirstLine { get; set; }
        public string MessagePath { get; set; }
        public int MessagePathNumber { get; set; }
        public int ContentLength { get; set; }
        public string Content { get; set; }
        private List<string> messagesList = new List<string>();
        public bool BodyExists { get; set; } = false;
        public string Response { get; set; }
        public int ResponseMessagesId { get; set; }
        public bool IsMessages { get; set; }
        //public string ResponseMessages { get; set; }
        public string AuthorizationToken { get; set; }

        public RequestContext(string inputMessage, List<string> list)
        {
            messagesList = list;                                                        //Teilen des erhaltenen String
            var messageSplitLine = inputMessage.Split("\r\n");                          //bei wird \r\n geteilt und diese werden herausgefiltert
            FirstLine = messageSplitLine[0];
            int rowCount = 2;
            int index = FindContentLength(messageSplitLine);
            FindAuthorization(messageSplitLine);
            Content = messageSplitLine[index + rowCount]; 
            
            

            if (BodyExists == true)
            {
                while (Content.Length != ContentLength)                                 //Bei mehr als einer Zeile in der Nachricht wird diese an den String Content angehängt
                {
                    rowCount++;
                    Content += "\r\n";
                    Content += messageSplitLine[index + rowCount];
                }
            }

            var messageSplit = FirstLine.Split(" ");                                    //Split bei " "
            HttpVerb = messageSplit[0];
            MessagePath = messageSplit[1];
            var messagePathSplit = MessagePath.Split("/");                              //Split bei "/"
        }


        private int FindContentLength(string[] messageSplitLine)
        {
            int index = 0;
            string var = "";

            for(int i = 0; i < messageSplitLine.Length; i++)
            {
                var = messageSplitLine[i];
                var var2 = var.Split(":");
                string var3 = var2[0];
                if(var3 == "Content-Length")
                {
                    var contentLengthString = var2[1];
                    ContentLength = Convert.ToInt32(contentLengthString);
                    index = i;
                    BodyExists = true;
                    return index;
                }
                else
                {
                    BodyExists = false;
                }
            }
            return index;
        }

        private void FindAuthorization(string[] messageSplitLine)
        {
            string var = "";

            for(int i = 0; i < messageSplitLine.Length; i++)
            {
                var = messageSplitLine[i];
                var var2 = var.Split(":");
                string var3 = var2[0];
                if(var3 == "Authorization")
                {
                    AuthorizationToken = var2[1];
                    return;
                }
            }
            return;
        }
    }
}
