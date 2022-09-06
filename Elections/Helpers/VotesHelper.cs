using Elections.Constants;
using Elections.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elections.Helpers
{
    public class VotesHelper
    {
        public static List<VotesModel> getArrangedVotes(List<List<string>> votesString)
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
                        if (++j < votesString[i].Count() &&
                            ValidCandidates.GetFullName(votesString[i][j]) != null)
                        {
                            vote.CandidateName = ValidCandidates.GetFullName(votesString[i][j]);
                        }
                        else
                            valid = false;
                    }
                    else
                    {
                        if (ValidCandidates.GetFullName(votesString[i][j]) != null)
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
                    if (valid)
                        result.Add(vote);
                }
            }
            return result;
        }
    }
}
