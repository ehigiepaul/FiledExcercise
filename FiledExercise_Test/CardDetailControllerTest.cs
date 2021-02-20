using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiledExercise.Controllers;
using FiledExercise.Data;
using FiledExercise.Models;
using FiledExercise.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FiledExercise_Test
{
    [TestFixture]
    public class CardDetailControllerTest
    {
        [SetUp]
        public void Setup()
        {

            var dbContext = new DbContextOptionsBuilder<FiledExerciseContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FiledExerciseContext;Trusted_Connection=True;MultipleActiveResultSets=true");
            _context = new FiledExerciseContext(dbContext.Options);
            _paymentGatwayRepo = new PaymentGatwayRepo(_context);
            _controller = new CardDetailController(_context, _paymentGatwayRepo);
        }

        [TearDown]
        public void Teardown()
        {
            _card = null;
            _paymentGatwayRepo = null;
            _context = null;
        }
        private FiledExerciseContext _context;
        private PaymentGatwayRepo _paymentGatwayRepo;
        private CardDetail _card;

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

            },   new CardDetail()
            {
                Amount = -30,
                CardHolder = "Paul Ehigie Paul",
                CreditCardNumber = "12345678901234",
                ExpirationDate = new DateTime(21, 12, 01),
                SecurityCode = "23s"

            },

        };

        private static IEnumerable<CardDetail> AllTest = new List<CardDetail>()
        {

        };

        private CardDetailController _controller;


        [Test]
        public void ProcessPaymentController_On_CardTransaction(CardDetail cardDetail)//[ValueSource("CCVerificationTestData")]
        {

            var paymentStatus = new PaymentState();
            paymentStatus.StateEnum = PaymentStateEnum.Pending;
            _context.SaveChangesAsync();
            var process = _paymentGatwayRepo.ProcessTransaction(cardDetail, paymentStatus).Result;

            if (process)
            {
                paymentStatus.StateEnum = PaymentStateEnum.Processed;
            }
            else
            {
                paymentStatus.StateEnum = PaymentStateEnum.Failed;
            }

            _context.SaveChangesAsync();
            Assert.IsTrue(process, "Transaction Failed");

        }
    }
}
