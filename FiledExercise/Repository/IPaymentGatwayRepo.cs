using System.Threading.Tasks;
using FiledExercise.Models;

namespace FiledExercise.Repository
{
    public interface IPaymentGatwayRepo
    {
        /**
         * use to handle transactions below $20
         * <param name="cardDetail"></param>
         */
        Task<bool> CheapTransaction(CardDetail cardDetail);
        /**
         * use to handle transactions between $21 - $500
         * <param name="cardDetail"></param>
         */
        Task<bool> ExpensiveTransaction(CardDetail cardDetail);
        /**
         * use to handle transactions above $500
         * <param name="cardDetail"></param>
         */
        Task<bool> PremiumTransaction(CardDetail cardDetail);
        Task<bool> ProcessTransaction(CardDetail cardDetail, PaymentState paymentStatus);
        bool CardVerification(CardDetail cardDetail);
    }
}