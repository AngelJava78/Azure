using AzureKeyVaultWebApp.Models;
using AzureKeyVaultWebApp.Models.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureKeyVaultWebApp.Controllers
{
    public class KeyVaultController : Controller
    {
        readonly IKeyVaultRepository<SecretDto> repository;
        public KeyVaultController()
        {
            repository = new KeyVaultRepository();
        }
        public IActionResult Index(string secretName)
        {
            var secrets = new List<SecretDto>();
            secrets = repository.GetAll();
            if (!string.IsNullOrWhiteSpace(secretName))
            {
                secrets = secrets.Where(secret => secret.Name.Contains(secretName)).ToList();
            }
            var model = new SecretViewModel
            {
                Secrets = secrets,
                SecretName = secretName
            };
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Value")] SecretDto secretDto)
        {
            if (ModelState.IsValid)
            {
                repository.Add(secretDto);
                return RedirectToAction(nameof(Index));
            }
            return View(secretDto);
        }

        public IActionResult Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var secretDto = repository.GetByName(id);
            if (secretDto == null)
            {
                return NotFound();
            }
            return View(secretDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Name,Value")] SecretDto secretDto)
        {
            if (id != secretDto.Name)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    repository.Update(secretDto);
                }
                catch (Exception)
                {
                    if (repository.GetByName(id) == null)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(secretDto);

        }

        public IActionResult Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var secret = repository.GetByName(id);
            if (secret == null)
            {
                return NotFound();
            }
            return View(secret);
        }

        public IActionResult Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }
            var secret = repository.GetByName(id);
            if (secret == null)
            {
                return NotFound();
            }
            return View(secret);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
