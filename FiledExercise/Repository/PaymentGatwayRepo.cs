using System;
using System.Threading.Tasks;
using FiledExercise.Data;
using FiledExercise.Models;

namespace FiledExercise.Repository
{
    public class PaymentGatwayRepo
    {
        private PaymentState _paymentStatus;
        private FiledExerciseContext _context;
        private int premiumTried;

        public PaymentGatwayRepo(FiledExerciseContext context)
        {
            _context = context;
        }
        /**
         * use to handle transactions below $20
         * <param name="cardDetail"></param>
         */
        public async Task<bool> CheapTransaction(CardDetail cardDetail)
        {
            try
            {
                switch (cardDetail)
                {
                    case null:
                        throw new Exception();
                    default:
                    {
                        if (cardDetail.Amount <= 20)
                        {
                            //some service run here
                            return true;
                        }
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                // logic of failed process
                Console.WriteLine(e);
                return false;
            }

        }

        /**
         * use to handle transactions between $21 - $500
         * <param name="cardDetail"></param>
         */
        public async Task<bool> ExpensiveTransaction(CardDetail cardDetail)
        {
            try
            {
                if (cardDetail.Amount >= 21 && cardDetail.Amount <= 500)
                {
                    //run logic on processed transaction
                    return true;
                }
                else
                {
                    await CheapTransaction(cardDetail);
                }
                return false;
            }
            catch (Exception e)
            {
                // logic to handle failed transaction
                Console.WriteLine(e);
                return false;
            }

        }

        /**
         * use to handle transactions above $500
         * <param name="cardDetail"></param>
         */
        public async Task<bool> PremiumTransaction(CardDetail cardDetail)
        {
            try
            {
                switch (premiumTried)
                {
                    case 3:
                        premiumTried = 3;
                        throw new Exception();
                    default:
                    {
                        //run a logic to process the premium transaction.

                        if (cardDetail == null)
                        {

                            throw new Exception();
                        }
                        else
                        {
                            if (cardDetail.Amount < 500)
                            {
                                return false;
                            }
                            return true;
                        }
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (premiumTried < 3)
                {
                    premiumTried++;
                    PremiumTransaction(cardDetail);
                }

                return false;
            }


        }


        public async Task<bool> ProcessTransaction(CardDetail cardDetail, PaymentState paymentStatus)
        {
            premiumTried = 0;
            _paymentStatus = paymentStatus;

            try
            {

                bool states;

                if (CardVerification(cardDetail))
                {
                    switch (cardDetail.Amount)
                    {
                        case <= 20:
                            states = await CheapTransaction(cardDetail);
                            break;
                        case > 20 and <= 500:
                            states = await ExpensiveTransaction(cardDetail);
                            break;
                        default:
                            states = await PremiumTransaction(cardDetail);
                            break;
                    }

                    _paymentStatus.StateEnum = states ? PaymentStateEnum.Processed : PaymentStateEnum.Failed;

                    _context.PaymentState.Update(_paymentStatus);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception();
                }

                return states;
            }
            catch (Exception e)
            {
                _paymentStatus.StateEnum = PaymentStateEnum.Failed;
                _context.PaymentState.Update(_paymentStatus);
                _context.SaveChangesAsync();
                Console.WriteLine(e);

                return false;
            }

        }

        public bool CardVerification(CardDetail cardDetail)
        {
            try
            {
                bool isCardOk = false;

                if (Convert.ToInt64(cardDetail.CreditCardNumber).GetType() == typeof(long))
                {
                    switch (cardDetail.CreditCardNumber.Length)
                    {
                        case 14:
                        case 15:
                        case 16:
                            isCardOk = true;
                            break;
                        default:
                            isCardOk = false;

                            break;
                    }

                    isCardOk = cardDetail.ExpirationDate > DateTime.Now //date check
                               && isCardOk // ccn check
                               && (String.IsNullOrEmpty(cardDetail.SecurityCode) || Convert.ToInt32(cardDetail.SecurityCode).ToString().Length <= 3) // date check
                               && cardDetail.Amount > 0; // amount check

                }

                return isCardOk;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }
    }
}
