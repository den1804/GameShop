﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Models;

namespace GameStore.WebUI.Controllers
{
    public class GameController : Controller
    {
        private IGameRepository _repository;
        public int pageSize = 4;
        public GameController(IGameRepository repo)
        {
            _repository = repo;
        }
        public ViewResult List(string category, int page = 1)
        {
            GameListViewModel model = new GameListViewModel
            {
                Games = _repository.Games
                    .Where(g => category == null || g.Category == category)
                    .OrderBy(game => game.GameId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ? _repository.Games.Count()
                        : _repository.Games.Where(game => game.Category == category).Count()
                },
                CurrentCategory = category
                
            };
            return View(model);
        }
        public FileContentResult GetImage(int gameId)
        {
            Game game = _repository.Games
                .FirstOrDefault(g => g.GameId == gameId);
            if (game != null)
                return File(game.ImageData, game.ImageMimeType);
            else
                return null;
        }
    }
    
}