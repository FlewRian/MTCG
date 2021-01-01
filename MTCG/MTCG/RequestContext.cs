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
        public string ResponseMessages { get; set; }

        public RequestContext(string inputMessage, List<string> list)
        {
            messagesList = list;                                                        //Teilen des erhaltenen String
            var messageSplitLine = inputMessage.Split("\r\n");                          //bei wird \r\n geteilt und diese werden herausgefiltert
            FirstLine = messageSplitLine[0];
            int rowCount = 2;
            int index = FindContentLength(messageSplitLine);
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

            if (HttpVerb == "GET")
            {
                var arrayLength = messagePathSplit.Length;
                IsMessages = MessagePathHandler(messagePathSplit, arrayLength);
                if (IsMessages)
                {
                    if (messagesList.Count >= MessagePathNumber && MessagePathNumber >= 0)
                    {
                        if (arrayLength == 2)                                           //wenn bei GET keine ID mit gegeben wird
                        {
                            for (int i = 0; i < messagesList.Count; i++)
                            {
                                ResponseMessages += (i + 1) + ": ";
                                ResponseMessages += messagesList[i];
                                ResponseMessages += "\r\n";
                            }
                        }
                        else
                        {
                            ResponseMessages = messagesList[MessagePathNumber - 1];    //Bei GET mit ID
                        }                                                             //Responsnachrichten an den Client
                        if (ResponseMessages != null)
                        {
                            Response = "HTTP/1.1 200 OK\r\nServer: RESTful-Server\r\nContent-Length: " + ResponseMessages.Length + "\r\n\r\n" + ResponseMessages;
                        }
                        else
                        {
                            Response = "HTTP/1.1 400 Bad Request\r\nServer: RESTful-Server\r\n";
                        }
                    }
                    else
                    {
                        Response = "HTTP/1.1 404 Not Found\r\nServer: RESTful-Server\r\n";
                    }
                }
                else
                {
                    Response = "HTTP/1.1 400 Bad Request\r\nServer: RESTful-Server\r\n";
                }
            }
            else if(HttpVerb == "POST")
            {
                if (ContentLength != 0)
                {
                    var arrayLength = 2;
                    IsMessages = MessagePathHandler(messagePathSplit, arrayLength);
                    if (IsMessages)
                    {
                        messagesList.Add(Content);
                        ResponseMessagesId = messagesList.Count;                     //Responsnachrichten an den Client
                        Response = "HTTP/1.1 201 Created\r\nServer: RESTful-Server\r\nContent-Length: " + ResponseMessagesId.ToString().Length + "\r\n\r\n" + ResponseMessagesId;
                    }
                    else
                    {
                        Response = "HTTP/1.1 400 Bad Request\r\nServer: RESTful-Server\r\n";
                    }
                }
                else
                {
                    Response = "HTTP/1.1 400 Bad Request\r\nServer: RESTful-Server\r\n";
                }
            }
            else if(HttpVerb == "PUT")
            {
                var arrayLength = messagePathSplit.Length;
                if (arrayLength == 2)                                               //Respons wenn keine ID mitgegeben wird
                {
                    Response = "HTTP/1.1 400 Bad Request\r\nServer: RESTful-Server\r\n";
                }
                else
                {
                    IsMessages = MessagePathHandler(messagePathSplit, arrayLength);
                    if (IsMessages)
                    {
                        if (messagesList.Count >= MessagePathNumber && MessagePathNumber >= 0)
                        {
                            messagesList[MessagePathNumber - 1] = Content;
                            Response = "HTTP/1.1 200 OK\r\nServer: RESTful-Server\r\n";          //Responsnachrichten an den Client
                        }
                        else
                        {
                            Response = "HTTP/1.1 404 Not Found\r\nServer: RESTful-Server\r\n";
                        }
                    }
                    else
                    {
                        Response = "HTTP/1.1 400 Bad Request\r\nServer: RESTful-Server\r\n";
                    }
                }
            }
            else if(HttpVerb == "DELETE")
            {
                var arrayLength = messagePathSplit.Length;
                if (arrayLength == 2)                                               //Respons wenn keine ID mitgegeben wird
                {
                    Response = "HTTP/1.1 400 Bad Request\r\nServer: RESTful-Server\r\n";
                }
                else
                {
                    IsMessages = MessagePathHandler(messagePathSplit, arrayLength);
                    if (IsMessages)
                    {
                        if (messagesList.Count >= MessagePathNumber && MessagePathNumber >= 0)
                        {
                            messagesList[MessagePathNumber - 1] = "";                           //Responsnachrichten an den Client
                            Response = "HTTP/1.1 200 OK\r\nServer: RESTful-Server\r\n";
                        }
                        else
                        {
                            Response = "HTTP/1.1 404 Not Found\r\nServer: RESTful-Server\r\n";
                        }
                    }
                    else
                    {
                        Response = "HTTP/1.1 400 Bad Request\r\nServer: RESTful-Server\r\n";
                    }
                }
            }
            else
            {
                Response = "HTTP/1.1 400 Bad Request\r\nServer: RESTful-Server\r\n";
            }
            
        }

        private bool MessagePathHandler(string[] messagePathSplit, int arrayLength)             
        {
            if(arrayLength == 2)
            {
                MessagePath = messagePathSplit[1];
                if (MessagePath != "messages")
                {
                    return false;
                }
                return true;
            }
            else
            {
                MessagePath = messagePathSplit[1];
                if (MessagePath != "messages")
                {
                    return false;
                }
                MessagePathNumber = Convert.ToInt32(messagePathSplit[2]);
                return true;
            }
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
    }
}
