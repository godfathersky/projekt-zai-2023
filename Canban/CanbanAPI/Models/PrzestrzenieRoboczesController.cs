using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

namespace CanbanAPI.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrzestrzenieRoboczesController : ControllerBase
    {
        private readonly CanbanDBContext _context;

        public PrzestrzenieRoboczesController(CanbanDBContext context)
        {
            _context = context;
        }

        [HttpGet("getLastId")]
        public async Task<int> GetLastTaskId()
        {
            var lastId = await _context.Zadania.OrderBy(x => x.IdZadanie).LastAsync();

            return (lastId.IdZadanie + 1);
        }

        [HttpGet("workSpaces/{user}")]
        public async Task<int> GetUserWorkSpaces(string user)
        {
            var workSpaces = await _context.Uzytkownicies.Where(x => x.NazwaUzytkownik == user).Include(x => x.PrzestrzenRoboczaUzytkownikas).ToListAsync();
            var amount = workSpaces[0].PrzestrzenRoboczaUzytkownikas.Count();
            return amount;
        }

        [HttpGet("userItems/{user}")]
        public async Task<UserSpacesAndBoards> GetUserSpace(string user)
        {
            var cUser = await _context.Uzytkownicies.FirstAsync(x => x.NazwaUzytkownik == user);
            var space = await _context.PrzestrzenRoboczaUzytkownikas.Where(x => x.IdUzytkownik == cUser.IdUzytkownika).ToListAsync();
            var userSpaceInfo = space[0].IdPrzestrzenRobocza;
            var spaceName = await _context.PrzestrzenieRoboczes.Where(x => x.IdPrzestrzenRobocza == userSpaceInfo).Include(x => x.TabliceZadans).ToListAsync();
            UserSpacesAndBoards usab = new UserSpacesAndBoards();
            usab.space = spaceName[0].NazwaPrzestrzenRobocza;
            var boardeasd = spaceName[0].TabliceZadans.ToList();
            var userLists = await _context.ListyZadans.Where(x => x.IdTablicaZadan == boardeasd[0].IdTablicaZadan).ToListAsync();
            List<SingleList> singles = new List<SingleList>();
            foreach(var item in userLists)
            {
                SingleList singleList = new SingleList();
                Console.WriteLine(item);
                singleList.nazwaLista = item.NazwaListaZadan;
                singleList.listaId = item.IdListaZadan;
                var itemTask = await _context.Zadania.Where(x => x.IdLista == item.IdListaZadan).ToListAsync();
                List<ListTask> taskList = new List<ListTask>();
                foreach (var task in itemTask)
                {
                    ListTask taskInfo = new ListTask();
                    taskInfo.taskName = task.NazwaZadanie;
                    taskInfo.taskId = task.IdZadanie;
                    taskList.Add(taskInfo);
                    singleList.zadaniaLista = taskList;
               }
                singles.Add(singleList);
            }
            usab.board = boardeasd[0].NazwaTablicaZadan;
            usab.listy = singles;
            return usab;
        }

        // GET: api/PrzestrzenieRoboczes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrzestrzenieRobocze>> GetPrzestrzenieRobocze(int id)
        {
          if (_context.PrzestrzenieRoboczes == null)
          {
              return NotFound();
          }
            var przestrzenieRobocze = await _context.PrzestrzenieRoboczes.FindAsync(id);

            if (przestrzenieRobocze == null)
            {
                return NotFound();
            }

            return przestrzenieRobocze;
        }

        // PUT: api/PrzestrzenieRoboczes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrzestrzenieRobocze(int id, PrzestrzenieRobocze przestrzenieRobocze)
        {
            if (id != przestrzenieRobocze.IdPrzestrzenRobocza)
            {
                return BadRequest();
            }

            _context.Entry(przestrzenieRobocze).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrzestrzenieRoboczeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PrzestrzenieRoboczes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PrzestrzenieRoboczesDTO>> PostPrzestrzenieRobocze(PrzestrzenieRoboczesDTO przestrzenieRobocze)
        {

            var user = await _context.Uzytkownicies.FirstAsync(x => x.NazwaUzytkownik == przestrzenieRobocze.User);

            PrzestrzenieRobocze pRobocze = new PrzestrzenieRobocze();

            string date = przestrzenieRobocze.DataUtworzeniaPrzestrzenRobocza;
            DateTime addDate = DateTime.Parse(date);

            if(przestrzenieRobocze.NazwaPrzestrzenRobocza.Trim() != "")
            {
                pRobocze.NazwaPrzestrzenRobocza = przestrzenieRobocze.NazwaPrzestrzenRobocza;
                pRobocze.DataUtworzeniaPrzestrzenRobocza = addDate;
            }

            _context.PrzestrzenieRoboczes.Add(pRobocze);
            await _context.SaveChangesAsync();

            var currentSpace = pRobocze.IdPrzestrzenRobocza;

            PrzestrzenRoboczaUzytkownika przestrzenRoboczaUzytkownika = new PrzestrzenRoboczaUzytkownika();

            przestrzenRoboczaUzytkownika.IdPrzestrzenRobocza = pRobocze.IdPrzestrzenRobocza;
            przestrzenRoboczaUzytkownika.IdUzytkownik = user.IdUzytkownika;

            _context.PrzestrzenRoboczaUzytkownikas.Add(przestrzenRoboczaUzytkownika);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("board")]
        public async Task<ActionResult<TabliceZadanDTO>> PostTabliceZadan(TabliceZadanDTO tabliceZadanDto)
        {

            var user = await _context.Uzytkownicies.Where(x => x.NazwaUzytkownik == tabliceZadanDto.User).Include(x => x.PrzestrzenRoboczaUzytkownikas).ToListAsync();

            var space = await _context.PrzestrzenieRoboczes.FirstAsync(x => x.NazwaPrzestrzenRobocza == tabliceZadanDto.NazwaPrzestrzen);


            TabliceZadan tablice = new TabliceZadan();

            string date = tabliceZadanDto.DataUtworzeniaTablicaZadan;
            DateTime addDate = DateTime.Parse(date);

            if (tabliceZadanDto.NazwaTablicaZadan != "")
            {
                tablice.NazwaTablicaZadan = tabliceZadanDto.NazwaTablicaZadan;
                tablice.DataUtworzeniaTablicaZadan = addDate;
                tablice.IdPrzestrzenRobocza = space.IdPrzestrzenRobocza;
            }

            _context.TabliceZadans.Add(tablice);
            await _context.SaveChangesAsync();

            return Ok("Utworzono nową tablicę");
        }

        //Zrobić tak, żeby była tablic pobierana na podstawie ID a nie nazwy.

        [HttpPost("list")]
        public async Task<ActionResult<ListCreateResponse>> PostListaZadan(ListaDTO lista)
        {
            var board = await _context.TabliceZadans.FirstAsync(x => x.NazwaTablicaZadan == lista.nazwaTablicaZadan);

            ListyZadan taskList = new ListyZadan();

            var date = lista.dataUtworzeniaListaZadan;
            DateTime currentDate = DateTime.Parse(date);

            taskList.IdTablicaZadan = board.IdTablicaZadan;
            taskList.NazwaListaZadan = lista.nazwaListaZadan;
            taskList.DataUtworzeniaListaZadan = currentDate;

            _context.ListyZadans.Add(taskList);
            await _context.SaveChangesAsync();

            var listId = taskList.IdListaZadan;

            ListCreateResponse response = new ListCreateResponse();

            response.response = "Lista utworzona pomyślnie";
            response.id = listId;

            return response;
        }

        [HttpPost("listTask")]
        public async Task<ActionResult<Zadanium>> PostTabliceZadan(listTaskDTO listaTask)
        {
            var user = await _context.Uzytkownicies.FirstAsync(x => x.NazwaUzytkownik == listaTask.user);

            Zadanium task = new Zadanium();

            var date = listaTask.dataUtworzenia;
            DateTime currentDate = DateTime.Parse(date);

            task.IdLista = listaTask.idLista;
            task.NazwaZadanie = listaTask.nazwaZadanie;
            task.DataUtworzenia = currentDate;

            _context.Zadania.Add(task);
            await _context.SaveChangesAsync();

            ZadaniaUzytkownika userTask = new ZadaniaUzytkownika();

            userTask.IdUzytkownik = user.IdUzytkownika;
            userTask.IdZadanie = task.IdZadanie;
            userTask.DataPrzydzielenieZadanie = currentDate;

            _context.ZadaniaUzytkownikas.Add(userTask);
            await _context.SaveChangesAsync();

            return Ok("Zadanie dodano pomyślnie");

        }

        [HttpDelete("DeleteTask")]
        public async Task<IActionResult> DeleteTask(DeletedTask dt)
        {
            var user = await _context.Uzytkownicies.FirstAsync(x => x.NazwaUzytkownik == dt.username);
            var userTask = await _context.ZadaniaUzytkownikas.FirstAsync(x => x.IdUzytkownik == user.IdUzytkownika && x.IdZadanie == dt.taskId);
            _context.ZadaniaUzytkownikas.Remove(userTask);
            await _context.SaveChangesAsync();
            var task = await _context.Zadania.FirstAsync(x => x.IdZadanie == dt.taskId);
            _context.Zadania.Remove(task);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/PrzestrzenieRoboczes/5
        [HttpDelete("deleteList")]
        public async Task<IActionResult> DeletePrzestrzenieRobocze(int id)
        {
            var list = await _context.ListyZadans.FirstAsync(x => x.IdListaZadan == id);
            _context.ListyZadans.Remove(list);
            await _context.SaveChangesAsync();
            return Ok("Udało się usunąć listę");
        }

        private bool PrzestrzenieRoboczeExists(int id)
        {
            return (_context.PrzestrzenieRoboczes?.Any(e => e.IdPrzestrzenRobocza == id)).GetValueOrDefault();
        }
    }
}
