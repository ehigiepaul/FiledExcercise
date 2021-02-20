using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiledExercise.Data;
using FiledExercise.Models;
using FiledExercise.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FiledExercise_Test
{
    [TestFixture]
    public class CardValidationTest
    {
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

        [SetUp]
        public void Setup()
        {

            var dbContext = new DbContextOptionsBuilder<FiledExerciseContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FiledExerciseContext;Trusted_Connection=True;MultipleActiveResultSets=true");
            _context = new FiledExerciseContext(dbContext.Options);
            _paymentGatwayRepo = new PaymentGatwayRepo(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _card = null;
            _paymentGatwayRepo = null;
            _context = null;
        }

        [Test]
        public void CheapTransaction_Below20_ReturnBool([ValueSource("CCTestData")] CardDetail cardDetail)
        {

            var cheap = _paymentGatwayRepo.CheapTransaction(cardDetail).Result;
            Assert.LessOrEqual(cardDetail.Amount, 20, "Transaction is greater than 20");
            Assert.IsTrue(cheap, "Transaction failed");
        }

        [Test]
        public void ExpensiveTransaction_Above20_ReturnBool([ValueSource("CCTestData")] CardDetail cardDetail)
        {
            var cheap = _paymentGatwayRepo.ExpensiveTransaction(cardDetail).Result;
            Assert.IsTrue(cardDetail.Amount <= 500 && cardDetail.Amount >= 21, "Transaction is less or greater than 21 and 500");
            Assert.IsTrue(cheap, "Transaction failed");
        }

        [Test]
        public void PremiumTransaction_Above20_ReturnBool([ValueSource("CCTestData")] CardDetail cardDetail)
        {

            var cheap = _paymentGatwayRepo.PremiumTransaction(cardDetail).Result;
            Assert.Greater(cardDetail.Amount, 500, "Transaction is less than 500");
            Assert.IsTrue(cheap, "Transaction failed");
        }

        [Test]
        public void CardVerification_When_Ok_ReturnBool([ValueSource("CCVerificationTestData")] CardDetail cardDetail)
        {

            var isCardVerified = _paymentGatwayRepo.CardVerification(cardDetail);
            Assert.IsTrue(isCardVerified, "Invalid card");
        }

        [Test]
        public void ProcessPaymentWhenInit([ValueSource("CCVerificationTestData")] CardDetail cardDetail)
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
