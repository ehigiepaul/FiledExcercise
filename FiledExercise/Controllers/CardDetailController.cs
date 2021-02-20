using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FiledExercise.Data;
using FiledExercise.Models;
using FiledExercise.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiledExercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardDetailController : ControllerBase
    {
        private readonly FiledExerciseContext _context;
        private readonly PaymentGatwayRepo _paymentGatwayRepo;

        public CardDetailController(FiledExerciseContext context, PaymentGatwayRepo paymentGatwayRepo)
        {
            _context = context;
            _paymentGatwayRepo = paymentGatwayRepo;

        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PaymentState>> ProcessPayment(CardDetail cardDetail)
        {
            try
            {
                //if validation ok process card payment
                if (ModelState.IsValid)
                {
                    var paymentStatus = new PaymentState();
                    paymentStatus.StateEnum = PaymentStateEnum.Pending;
                    _context.PaymentState.Add(paymentStatus);
                    await _context.SaveChangesAsync();

                    var processPayment = new PaymentGatwayRepo(_context);

                    if (paymentStatus != null)
                    {
                        var isProcessed = await _paymentGatwayRepo.ProcessTransaction(cardDetail, paymentStatus);

                    }

                    return Ok("Payment is processed");
                }
                else
                {
                    return BadRequest(new
                    {
                        message = "The request is invalid"
                    });
                }

            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    error= e.Message,
                    message = "Lost connection to payment platform, Please try back later"
                });
            }

        }
    }
}
