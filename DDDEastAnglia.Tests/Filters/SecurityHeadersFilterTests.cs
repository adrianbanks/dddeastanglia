﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DDDEastAnglia.App_Start.Filters;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Filters
{
    [TestFixture]
    public class SecurityHeadersFilterTests
    {
        private HttpContextBase contextBase;
        private HttpResponseBase responseBase;
        private NameValueCollection headers;
        private ResultExecutingContext context;

        [SetUp]
        public void Setup()
        {
            contextBase = Substitute.For<HttpContextBase>();
            responseBase = Substitute.For<HttpResponseBase>();
            headers = new NameValueCollection();
            responseBase.Headers.Returns(headers);
            contextBase.Response.Returns(responseBase);

            context = new ResultExecutingContext
            {
                HttpContext = contextBase
            };
        }

        [TestCase("x-powered-by", "ASP.NET")]
        [TestCase("x-aspnet-version", "4.0.30319")]
        [TestCase("x-aspnetmvc-version", "4.0")]
        public void ASPNET_Header_Is_Removed(string headerName, string headerValue)
        {
            headers.Add(headerName,headerValue);

            SecurityHeadersFilter filter = new SecurityHeadersFilter();

            filter.OnResultExecuting(context);

            NameValueCollection filteredHeaders = responseBase.Headers;
            
            Assert.That(filteredHeaders[headerName], Is.Null);
        }

        [Test]
        public void Security_Header_Is_Added()
        {
            SecurityHeadersFilter filter = new SecurityHeadersFilter();

            filter.OnResultExecuting(context);

            NameValueCollection filteredHeaders = responseBase.Headers;

            Assert.That(filteredHeaders["X-Frame-Origins"], Is.Not.Null);
        }

        [Test]
        public void Security_Header_Is_Correct_Value()
        {
            SecurityHeadersFilter filter = new SecurityHeadersFilter();

            filter.OnResultExecuting(context);

            NameValueCollection filteredHeaders = responseBase.Headers;

            Assert.That(filteredHeaders["X-Frame-Origins"], Is.EqualTo("SAMEORIGIN"));
        }
    }
}