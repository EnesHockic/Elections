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

            if(file != null)
            {
                var parsedVotes = await _csvFileParser.GetRecordsAsString(file);
                var votes = getArrangedVotes(parsedVotes);
                var newConstituencies = await _constituencyService.UpsertConstituencies(votes);
                var newCandidates = await _candidateService.UpsertCandidates(votes);
                var matchedModel = await MatchModelWithExistingData(votes);
                var newVotes = await _votesService.StoreVotes(votes);
                return "Success";
            }
            return "File is not passed!";
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

        private List<VotesModel> getArrangedVotes(List<List<string>> votesString)
        {
            var result = new List<VotesModel>();
            for (int i = 0; i < votesString.Count(); i++)
            {
                for (int j = 1; j < votesString[i].Count(); j++)
                {
                    var vote = new VotesModel();
                    vote.Constituency = votesString[i][0];
                    var numOfVotes = 0;
                    var valid = true;
                    if (Int32.TryParse(votesString[i][j], out numOfVotes))
                    {
                        vote.NumberOfVotes = numOfVotes;
                        if(++j < votesString[i].Count() &&
                            ValidCandidates.GetFullName(votesString[i][j]) != null)
                        {
                            vote.CandidateName = ValidCandidates.GetFullName(votesString[i][j]);
                        }
                        else
                            valid = false;
                    }
                    else
                    {
                        if(ValidCandidates.GetFullName(votesString[i][j]) != null)
                        {
                            vote.CandidateName = ValidCandidates.GetFullName(votesString[i][j]);
                            if (++j < votesString[i].Count())
                            {
                                if (Int32.TryParse(votesString[i][j], out numOfVotes))
                                    vote.NumberOfVotes = Int32.Parse(votesString[i][j]);
                                else
                                    vote.Error = "Invalid number of votes!";
                            }
                            else
                                vote.Error = "Invalid format!";
                        }
                        else 
                            valid = false;
                    }
                    if(valid)
                        result.Add(vote);
                }
            }
            return result;
        }
    }
}
