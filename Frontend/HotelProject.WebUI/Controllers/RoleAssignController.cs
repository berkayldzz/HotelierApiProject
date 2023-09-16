using HotelProject.EntityLayer.Concrete;
using HotelProject.WebUI.Models.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelProject.WebUI.Controllers
{
    public class RoleAssignController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleAssignController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var values = _userManager.Users.ToList();
            return View(values);
        }
        [HttpGet]
        public async Task<IActionResult> AssignRole(int id)
        {
            // HttpGet teki bir mettotan httppost tarafına veri taşımak için tempdatadan faydalanıcaz.
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            TempData["userid"] = user.Id;
            var roles = _roleManager.Roles.ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();
           
            // Bu döngü rollere tek tek gidip bakacak ve bu rollerin  hepsini oluşturmuş olduğum listenin içersine ekleyecek.
            // Önemli nokta : Eklerken bu kullanıcı bu role sahip mi değil mi onu da ekleyecek.
            
            foreach (var item in roles)
            {
                RoleAssignViewModel model = new RoleAssignViewModel();
                model.RoleID = item.Id;
                model.RoleName = item.Name;
                model.RoleExist = userRoles.Contains(item.Name);  // O role sahip mi değil mi
                roleAssignViewModels.Add(model);   // listenin içersine ekliyor.
            }
            return View(roleAssignViewModels);
        }
        [HttpPost]

        // Birden fazla rol üzerinde işlem yapabiliriz o yüzden parametremiz List türünde
        public async Task<IActionResult> AssignRole(List<RoleAssignViewModel> roleAssignViewModel) 
        {
            var userid = (int)TempData["userid"];
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userid);
            foreach (var item in roleAssignViewModel)
            {
                if (item.RoleExist)  // checkbox seçiliyse yani
                {
                    await _userManager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.RoleName);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
