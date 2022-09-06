using Elections.Application.Interfaces;
using Elections.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elections.Constants;
using Elections.Helpers;

namespace Presidental_Election.Controllers
{
    public class ElectionController : Controller
    {
        ICsvFileParser _csvFileParser { get; set; }
        IVotesService _votesService { get; set; }
        IConstituencyService _constituencyService { get; set; }
        ICandidateService _candidateService { get; set; }
        public ElectionController(ICsvFileParser csvFileParser, IVotesService votesService,
            IConstituencyService constituencyService, ICandidateService candidateService)
        {
            _csvFileParser = csvFileParser;
            _votesService = votesService;
            _constituencyService = constituencyService;
            _candidateService = candidateService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _votesService.GetVotesAsViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<string> UploadResult(IFormFile file)
        {

            try
            {
                if(file != null)
                {
                    var parsedVotes = await _csvFileParser.GetRecordsAsString(file);
                    var votes =VotesHelper.getArrangedVotes(parsedVotes);
                    if(votes != null)
                    {
                        var newConstituencies = await _constituencyService.UpsertConstituencies(votes);
                        var newCandidates = await _candidateService.UpsertCandidates(votes);
                        var matchedModel = await MatchModelWithExistingData(votes);
                        var newVotes = await _votesService.StoreVotes(votes);
                        return "Success";
                    }
                }
                return "File is empty or not sent!";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
        async Task<List<VotesModel>> MatchModelWithExistingData(List<VotesModel> model)
        {
            var existingCandidates = await _candidateService.GetAll();
            var existingConstituencies = await _constituencyService.GetAll();
            foreach (var item in model)
            {
                item.CandidateId = existingCandidates.First(x => x.Name == item.CandidateName).Id;
                item.ConstituencyId = existingConstituencies.First(x => x.Name == item.Constituency).Id;
            }
            return model;
        }
    }
}
