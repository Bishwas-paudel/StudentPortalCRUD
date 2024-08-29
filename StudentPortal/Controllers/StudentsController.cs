using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Models;
using StudentPortal.Models.Entities;
using StudentPortal.web.Data;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace StudentPortal.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        // Store the dbContext instance in a private field
        public StudentsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddstudentViewModel viewModel)
        {
            var student = new Student
            {
                Name = viewModel.Name,
                email = viewModel.email,
                phone = viewModel.phone,
                city = viewModel.city
            };

            // Use the _dbContext instance
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var students = await _dbContext.Students.ToListAsync();

            return View(students);


        }

        [HttpGet]
        public async Task<IActionResult> edit(Guid id)
        {
            var student = await _dbContext.Students.FindAsync(id);
            return View(student);
        }
        [HttpPost]
        public async Task<IActionResult> edit(Student viewModel)
        {
            var student = await _dbContext.Students.FindAsync(viewModel.Id);

            if (student != null)
            {
                student.Name = viewModel.Name;
                student.email = viewModel.email;
                student.phone = viewModel.phone;
                student.city = viewModel.city;
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Student viewModel) {
            var student = await _dbContext.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(x=>x.Id == viewModel.Id);
            if(student != null)
            {
                _dbContext.Students.Remove(viewModel);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");

        }
    }
}


