using ExpenseApi.Data;
using ExpenseApi.Models;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ExpenseApi.Controllers
{
    [EnableCors("*","*","*")]
    public class EntriesController : ApiController
    {
        public IHttpActionResult GetEntries()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var entries = context.Entries.ToList();
                    return Ok(entries);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
        public IHttpActionResult PostEntry([FromBody] Entry entity)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                using (var context = new AppDbContext())
                {
                    context.Entries.Add(entity);
                    context.SaveChanges();
                    return Ok("Entry was created");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public IHttpActionResult Put(int id, [FromBody] Entry entry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != entry.Id) return BadRequest();
            try
            {
                using (var context = new AppDbContext())
                {
                    var oldEntry = context.Entries.FirstOrDefault(x => x.Id == id);
                    if (oldEntry == null) return NotFound();
                    oldEntry.Description = entry.Description;
                    oldEntry.IsExpense = entry.IsExpense;
                    oldEntry.Value = entry.Value;
                    context.SaveChanges();
                    return Ok("Entry Updated");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
