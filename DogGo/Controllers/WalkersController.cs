using DogGo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using DogGo.Repositories;
using DogGo.Models.ViewModels;
using System.Security.Claims;
using System;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        // GET: WalkersController
        public ActionResult Index()
        {
            try
            {
                int ownerId = GetCurrentUserId();
                Owner owner = _ownerRepo.GetOwnerById(ownerId);
                List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);
                return View(walkers);
            }
            catch (Exception)
            {
                List<Walker> walkers = _walkerRepo.GetAllWalkers();

                return View(walkers);
            }
        }

        // GET: WalkersController/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Walks> walks = _walkRepo.GetWalksByWalkerId(id);

            if (walker == null)
            {
                return NotFound();
            }

            WalkerProfileView wpv = new WalkerProfileView()
            {
                Walker = walker,
                Walks = walks
            };

            return View(wpv);
        }

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;
        private readonly IOwnerRepository _ownerRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository, IOwnerRepository ownerRepository, IWalkRepository walkRepository)
        {
            _walkerRepo = walkerRepository;
            _ownerRepo = ownerRepository;
            _walkRepo = walkRepository;
        }
    }
}
