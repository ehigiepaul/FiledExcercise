using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FiledExercise.Controllers;
using FiledExercise.Data;
using FiledExercise.Models;
using FiledExercise.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FiledExercise_Test
{
    [TestFixture]
    public class CardDetailControllerTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var dbContext = new DbContextOptionsBuilder<FiledExerciseContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FiledExerciseContext;Trusted_Connection=True;MultipleActiveResultSets=true");
            _context = new FiledExerciseContext(dbContext.Options);
            _paymentProcessController = new ProcessPaymentController(_context);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            _context = null;
            _paymentProcessController = null;
        }
        private FiledExerciseContext _context;
        private ProcessPaymentController _paymentProcessController;


        private static IEnumerable<CardDetail> CCTestData = new List<CardDetail>()
        {
            new CardDetail()
            {
                Amount = 20,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(2021, 12, 01)
            },
            null,
            new CardDetail()
            {
                Amount = 21,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(2021, 12, 01)
            },
            new CardDetail()
            {
                Amount = 499,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(2021, 12, 01)
            },
            new CardDetail()
            {
                Amount = 600,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(2021, 12, 01)
            }

        };

        private static IEnumerable<CardDetail> CCVerificationTestData = new List<CardDetail>()
        {
            new CardDetail()
            {
                Amount = 20,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(2021, 12, 01)
            },
            null,
            new CardDetail()
            {
                Amount = 21,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(2021, 12, 01)
            },
            new CardDetail()
            {
                Amount = 499,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(2021, 12, 01)
            },
            new CardDetail()
            {
                Amount = 600,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(2021, 12, 01)
            },
            new CardDetail()
            {
                Amount = 20,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "1234567890ab234",
                ExpirationDate = new DateTime(2021, 12, 01)
            },
            null,
            new CardDetail()
            {
                Amount = 21,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "1234567890123",
                ExpirationDate = new DateTime(2021, 12, 01)
            },
            new CardDetail()
            {
                Amount = 499,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234567",
                ExpirationDate = new DateTime(2021, 12, 01)
            },
            new CardDetail()
            {
                Amount = -30,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(2021, 12, 01)
            },
            new CardDetail()
            {
                Amount = -30,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(2020, 12, 01)
            },
            new CardDetail()
            {
                Amount = -30,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(2021, 12, 01),
                SecurityCode = "233"

            },
            new CardDetail()
            {
                Amount = -30,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(21, 12, 01),
                SecurityCode = "2335"

            },
            new CardDetail()
            {
                Amount = -30,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(21, 12, 01),
                SecurityCode = "23s"

            },

        };


        [Test]
        public void ProcessPaymentController_On_CardTransaction( CardDetail cardDetail) //[ValueSource("CCVerificationTestData")]
        {

            var responseResult = _paymentProcessController.ProcessPayment(cardDetail).Result;


            Assert.IsTrue(responseResult.Value, "Transaction failed");

        }
    }
}
