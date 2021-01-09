using MTCG;
using NUnit.Framework;
using System.Collections.Generic;

namespace MTCG_Test
{
    public class HTTP_RestServerTests
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
    }
}