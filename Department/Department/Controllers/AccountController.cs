using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Department.Models;
using Department.Models.AccountViewModels;
using Department.Services;
using Department.Data;
using Microsoft.EntityFrameworkCore;

namespace Department.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext applicationDbContext,
            IEmailSender emailSender,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDbContext = applicationDbContext;
            _emailSender = emailSender;
            _logger = logger;
        }


        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> IndexD(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var depart = await _applicationDbContext.Departs.SingleOrDefaultAsync(m => m.ID == id);
            if (depart == null)
            {
                return NotFound();
            }
            return View(depart);
        }

        public async Task<IActionResult> IndexS(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var stu = await _applicationDbContext.Students.SingleOrDefaultAsync(m => m.ID == id);
            if (stu == null)
            {
                return NotFound();
            }
            List<long> Dids = new List<long>();
            List<Activity> activities = new List<Activity>();
            List<DNameandDuty> dNameandDuties = new List<DNameandDuty>();
            var dtoMMappings = await _applicationDbContext.DtoMMappings.Where(m => m.MemberID == id).ToListAsync(); 
            foreach( DtoMMapping dtoMMaping in dtoMMappings)
            {
                Dids.Add(dtoMMaping.DepartID);
                DNameandDuty d = new DNameandDuty { DName = dtoMMaping.DepartName, Duty = dtoMMaping.Duty };
                dNameandDuties.Add(d);
            }
            foreach(long Did in Dids)
            {
                List<Activity> activity = await _applicationDbContext.Activities.Where(a => a.DepartID == Did).ToListAsync();
                activities.AddRange(activity);
            }

            IndexSViewModel indexSViewModel = new IndexSViewModel
            {
                Stu = stu,
                DNameandDuties = dNameandDuties,
                Activities = activities
            };
            return View(indexSViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditD(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var depart = await _applicationDbContext.Departs.SingleOrDefaultAsync(m => m.ID == id);
            if (depart == null)
            {
                return NotFound();
            }
            return View(depart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditD(long id, [Bind("ID,Email,Name,Minster,Vice,QQ,Introduction")] Depart depart)
        {
            if(id != depart.ID )
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                try
                {
                    _applicationDbContext.Departs.Update(depart);
                    await _applicationDbContext.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!DepartmentExists(depart.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("IndexD", new { id });
            }
            return View("Home", "Error");
        }

        [HttpGet]
        public async Task<IActionResult> EditS(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var stu = await _applicationDbContext.Students.SingleOrDefaultAsync(m => m.ID == id);
            if (stu == null)
            {
                return NotFound();
            }
            return View(stu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditS(long id,[Bind("Email,ID,Gender,Name,Introduction,StudentID,Grade,Institute")] Student stu)
        {
            if (id != stu.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _applicationDbContext.Students.Update(stu);
                    await _applicationDbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(stu.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("IndexS", new { id });
            }
            return View("Home","Error");
        }

        //还没实现的功能
        public IActionResult ShowD(long Did,long Sid,long Aid)
        {
            Depart depart = _applicationDbContext.Departs.Find(Did);
            return View(depart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowD(long Did,long Sid, long Aid, bool enabled)
        {
            DtoAMapping dtoAMapping = await _applicationDbContext.DtoAMappings.Where(d => d.ApplicationID == Aid || d.StudentID == Sid).FirstOrDefaultAsync();
            dtoAMapping.Enabled = enabled;
            _applicationDbContext.DtoAMappings.Update(dtoAMapping);
            long id = Sid;
            return RedirectToAction("Apply", new { id });
        }

        public IActionResult Apply(long id)
        {
            ViewBag.id = id;
            Student stu = _applicationDbContext.Students.SingleOrDefault(s => s.ID == id);
            List<DtoMMapping> departs = _applicationDbContext.DtoMMappings.Where(d => d.MemberID == id).ToList();
            List<long> departids = new List<long>();
            foreach(DtoMMapping depart in departs)
            {
                departids.Add(depart.ID);
            }
            List<Application> applications = _applicationDbContext.Applications.Where(a => (a.Grade == stu.Grade) || (a.Institute == stu.Institute) || 
            (a.Enabled == true) || !(departids.Contains(a.DepartID))).Select(a => a).ToList();
            DtoAMapping dtoAMapping = new DtoAMapping();
            foreach(Application application in applications)
            {
                if(_applicationDbContext.DtoAMappings.Where(d => d.ApplicationID == application.ID || d.StudentID == id) == null)
                {
                    dtoAMapping.ApplicationID = application.ID;
                    dtoAMapping.DepartID = application.DepartID;
                    dtoAMapping.StudentID = id;
                    dtoAMapping.Duty = "";
                    dtoAMapping.Enabled = false;
                    _applicationDbContext.DtoAMappings.Add(dtoAMapping);
                }
            }
            List<ApplyViewModel> applyViewModels = new List<ApplyViewModel>();
            for(int i = 0; i< applications.Count(); i++)
            {
                foreach (ApplyViewModel applyviewmodel in applyViewModels)
                {
                    applyviewmodel.DepartName = applications[i].DName;
                    applyviewmodel.ApplicationID = applications[i].ID;
                    applyviewmodel.DepartID = applications[i].DepartID;
                    applyviewmodel.Address = applications[i].Address;
                    applyviewmodel.Time = applications[i].Time;
                    DtoAMapping dtoa = _applicationDbContext.DtoAMappings.Where(d => d.ApplicationID == applications[i].ID || d.StudentID == id).FirstOrDefault();
                    applyviewmodel.Enabled = dtoa.Enabled;
                }
            }
            return View(applyViewModels);
        }

        public IActionResult Applications(long id)
        {
            ViewBag.id = id;
            List<DtoAMapping> dtoAMappings = _applicationDbContext.DtoAMappings.Where(d => d.DepartID == id).ToList();
            List<long> stuids = new List<long>();
            List<Student> stus = new List<Student>();
            foreach(DtoAMapping dtoAMapping in dtoAMappings)
            {
                stuids.Add(dtoAMapping.StudentID);
            }
            stuids = stuids.Distinct().ToList();
            foreach(long stuid in stuids)
            {
                stus.Add(_applicationDbContext.Students.Where(s => s.ID == stuid).FirstOrDefault());
            }
            return View(stuids);
        }

        public IActionResult AppliyDetails(long id)
        {
            return View();
        }
        

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    if(model.Kind == "Department")
                    {
                        long? id = _applicationDbContext.Departs.Where(s => s.Email == model.Email).FirstOrDefault().ID;
                        if (id == null)
                        {
                            return NotFound();
                        }
                        return RedirectToAction("IndexD",new { id });
                    }
                    else
                    {
                        long? id = _applicationDbContext.Students.Where(s => s.Email == model.Email).FirstOrDefault().ID;
                        if(id == null)
                        {
                            return NotFound();
                        }
                        return RedirectToAction("IndexS",new { id });
                    }
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

       
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Kind = model.Kind };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
                    await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User created a new account with password.");
                    if (model.Kind == "Depart")
                    {
                        var depart = new Depart { Email = model.Email };
                        await _applicationDbContext.Departs.AddAsync(depart);
                        await _applicationDbContext.SaveChangesAsync();
                        long id = depart.ID;
                        return RedirectToAction( "IndexD", new { id });
                    }
                    else
                    {
                        var stu = new Student { Email = model.Email };
                        await _applicationDbContext.Students.AddAsync(stu);
                        await _applicationDbContext.SaveChangesAsync();
                        long id = stu.ID;
                        return RedirectToAction("IndexS", new { id });
                    }
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private bool StudentExists(long id)
        {
            return _applicationDbContext.Students.Any(e => e.ID == id);
        }

        private bool DepartmentExists(long id)
        {
            return _applicationDbContext.Departs.Any(e => e.ID == id);
        }


        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion
    }
}
