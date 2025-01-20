// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CWhiteH60Customer.DAL;
using CWhiteH60Customer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CWhiteH60Customer.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICustomerRepository<Customer> _customerRepository;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ICustomerRepository<Customer> customerRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customerRepository = customerRepository;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            
            [RegexStringValidator("^[a-zA-Z'-\\s]+$")]
            [StringLength(20)]
            [Display(Name = "First Name")]
            public string? FirstName { get; set; }
            
            [RegexStringValidator("^[a-zA-Z'-\\s]+$")]
            [StringLength(30)]
            [Display(Name = "Last Name")]
            public string? LastName { get; set; }
            
            [EmailAddress]
            public string Email { get; set; }
            
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            
            [MaxLength(2)]
            [StringLength(2)]
            public string? Province { get; set; }
            
            [StringLength(16)]
            [Display(Name = "Credit Card")]
            public string? CreditCard { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            
            var customer = await _customerRepository.GetByUserId(user.Id);
            
            ViewData["Provinces"] = new Dictionary<string, string>() {
                { "QC", "Quebec" },
                { "ON", "Ontario" },
                { "NB", "New Brunswick" },
                { "MB", "Manitoba" },
            };

            Input = new InputModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = phoneNumber,
                Province = customer.Province,
                CreditCard = customer.CreditCard
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault();
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid) {
                await LoadAsync(user);
                return Page();
            }
            
            var customer = await _customerRepository.GetByUserId(user.Id);
            
            if (Input.FirstName != customer.FirstName) {
                customer.FirstName = Input.FirstName;
            }

            if (Input.LastName != customer.LastName) {
                customer.LastName = Input.LastName;
            }

            if (Input.Email != customer.Email) {
                customer.Email = Input.Email;
            }

            if (Input.PhoneNumber != customer.PhoneNumber) {
                customer.PhoneNumber = Input.PhoneNumber;
            }

            if (Input.Province != customer.Province) {
                customer.Province = Input.Province;
            }

            if (Input.CreditCard != customer.CreditCard) {
                customer.CreditCard = Input.CreditCard;
            }

            await _customerRepository.Update(customer);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
