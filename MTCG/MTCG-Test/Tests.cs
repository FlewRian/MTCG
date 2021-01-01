using MTCG;
using NUnit.Framework;
using System.Collections.Generic;

namespace MTCG_Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void HTTPVerbIsPOST()
        {
            var inputString = @"POST /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 4

Test";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("POST", requestContext.HttpVerb);
        }

        [Test]
        public void HTTPVerbIsGET()
        {
            var inputString = @"GET /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 0

";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("GET", requestContext.HttpVerb);
        }

        [Test]
        public void HTTPVerbIsPUT()
        {
            var inputString = @"PUT /messages/1 HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 0

";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("PUT", requestContext.HttpVerb);
        }

        [Test]
        public void HTTPVerbIsDELETE()
        {
            var inputString = @"DELETE /messages/1 HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 0

";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("DELETE", requestContext.HttpVerb);
        }

        [Test]
        public void NoHTTPVerb()
        {
            var inputString = @"ABC /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 0

";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("HTTP/1.1 400 Bad Request\r\nServer: RESTful-Server\r\n", requestContext.Response);
        }

        [Test]
        public void MessagesPathIsMessages()
        {
            var inputString = @"POST /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 4

Test";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual(true, requestContext.IsMessages);

        }

        [Test]
        public void MessagesPathIsNotMessages()
        {
            var inputString = @"POST /nachricht HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 0

";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual(false, requestContext.IsMessages);

        }
        [Test]
        public void PUTMessageIDIsMissing()
        {
            var inputString = @"PUT /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 0

";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("HTTP/1.1 400 Bad Request\r\nServer: RESTful-Server\r\n", requestContext.Response);
        }

        [Test]
        public void PUTMessageIDOutOfScope()
        {
            var inputString = @"PUT /messages/5 HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 0

";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("HTTP/1.1 404 Not Found\r\nServer: RESTful-Server\r\n", requestContext.Response);
        }

        [Test]
        public void POSTResponse201CreatedPlusMessageID()
        {
            var inputString = @"POST /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 4

Test";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("HTTP/1.1 201 Created\r\nServer: RESTful-Server\r\nContent-Length: 1\r\n\r\n1", requestContext.Response);
        }

        [Test]
        public void GETAllMessages()
        {
            var inputString = @"GET /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 0

";
            var list = new List<string>();
            list.Add("Test");
            list.Add("Test2\r\nmit zwei Zeilen");
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("HTTP/1.1 200 OK\r\nServer: RESTful-Server\r\nContent-Length: 36\r\n\r\n1: Test\r\n2: Test2\r\nmit zwei Zeilen\r\n", requestContext.Response);
        }

        [Test]
        public void BodyExistsTrue()
        {
            var inputString = @"POST /messages HTTP/1.1
Content-Length: 0

";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual(true, requestContext.BodyExists);
        }

        [Test]
        public void BodyExistsFlase()
        {
            var inputString = @"GET /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive

";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual(false, requestContext.BodyExists);
        }

        [Test]
        public void POSTResponsMessageIDIs1()
        {
            var inputString = @"POST /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 4

Test";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual(1, requestContext.ResponseMessagesId);
        }
        [Test]
        public void GETMessagePathIsMessages()
        {
            var inputString = @"GET /messages/1 HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 0

";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("messages", requestContext.MessagePath);
        }

        [Test]
        public void POSTContent()
        {
            var inputString = @"POST /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 16

Test für Content";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("Test für Content", requestContext.Content);
        }

        [Test]
        public void POSTContentWithTowLines()
        {
            var inputString = @"POST /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 33

Test für Content
mit zwei Zeilen";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("Test für Content\r\nmit zwei Zeilen", requestContext.Content);
        }

        [Test]
        public void ContenLengthIs22()
        {
            var inputString = @"POST /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 22

Test für ContentLength";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual(22, requestContext.ContentLength);
        }

        [Test]
        public void FirstLineTest()
        {
            var inputString = @"POST /messages HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 22

Test für ContentLength";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual("POST /messages HTTP/1.1", requestContext.FirstLine);
        }

        [Test]
        public void MessagesPathNumberis42()
        {
            var inputString = @"GET /messages/42 HTTP/1.1
User-Agent: PostmanRuntime/7.26.5
Accept: */*
Postman-Token: 963ecedd-fdba-4eb5-b7ec-8a8ff1b3140a
Host: localhost:8000
Accept-Encoding: gzip, deflate, br
Connection: keep-alive
Content-Length: 0

";
            var list = new List<string>();
            var requestContext = new RequestContext(inputString, list);

            Assert.AreEqual(42, requestContext.MessagePathNumber);
        }
    }
}