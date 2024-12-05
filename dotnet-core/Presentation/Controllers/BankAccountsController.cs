using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using XMP.Application.DTOs;
using XMP.Application.Interfaces;

namespace XMP.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AxisBankTransactionsController : ControllerBase
    {
        private readonly IAxisBankTransactionService _transactionService;

        public AxisBankTransactionsController(IAxisBankTransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                if (pageNumber <= 0 || pageSize <= 0)
                {
                    return BadRequest("Invalid page number or page size.");
                }

                var transactions = await _transactionService.GetAllTransactionsAsync(pageNumber, pageSize);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    return NotFound();
                }
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AxisBankTransactionDto transactionDto)
        {
            try
            {
                if (transactionDto == null)
                {
                    return BadRequest("Transaction data is null.");
                }

                await _transactionService.AddTransactionAsync(transactionDto);
                return CreatedAtAction(nameof(GetById), new { id = transactionDto.Id }, transactionDto);
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AxisBankTransactionDto transactionDto)
        {
            try
            {
                if (transactionDto == null || id != transactionDto.Id)
                {
                    return BadRequest("Transaction data is invalid.");
                }

                await _transactionService.UpdateTransactionAsync(transactionDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _transactionService.DeleteTransactionAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception details
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
