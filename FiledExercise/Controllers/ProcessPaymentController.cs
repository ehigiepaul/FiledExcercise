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
    public class ProcessPaymentController : ControllerBase
    {
        private readonly FiledExerciseContext _context;
        private PaymentGatwayRepo _paymentGatewayRepo;

        public ProcessPaymentController(FiledExerciseContext context)
        {
            _context = context;
            _paymentGatewayRepo = new PaymentGatwayRepo(_context);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> ProcessPayment(CardDetail cardDetail)
        {
            try
            {
                if (cardDetail == null)
                {
                    throw new Exception();
                }

                    var paymentStatus = new PaymentState();
                    paymentStatus.StateEnum = PaymentStateEnum.Pending;
                    _context.PaymentState.Add(paymentStatus);
                    await _context.SaveChangesAsync();

                    var isProcessed = await _paymentGatewayRepo.ProcessTransaction(cardDetail, paymentStatus);

                    if (isProcessed)
                    {
                        return Ok("Payment is processed");
                    }

                return BadRequest(new
                {
                    message = "The request is invalid"
                });

            }
            catch (Exception e)
            {
                return StatusCode(500, new
                {
                    error = e.Message,
                    message = "Lost connection to payment platform, Please try back later"
                });
            }

        }
    }
}
