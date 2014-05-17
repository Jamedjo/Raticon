using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raticon.Service
{
    public class LookupChoice
    {
        public enum Action { NewSearch, MoreResults, GiveUp }

        private string imdbId;
        private string newTitleToSearch;
        private LookupChoice.Action action;

        public LookupChoice(LookupResult choice)
        {
            this.imdbId = choice.ImdbId;
        }
        public LookupChoice(string imdbId)
        {
            this.imdbId = imdbId;
        }

        /// <summary>
        /// A Retry LookupChoice to retry with a new title.
        /// </summary>
        /// <param name="action">MUST be LookupChoice.Action.NewSearch</param>
        /// <param name="newTitleToSearch">The new title to retry searching for</param>
        public LookupChoice(LookupChoice.Action action, string newTitleToSearch)
        {
            if (action!=LookupChoice.Action.NewSearch)
            {
                throw new Exception("Invalid. Action must be set to LookupChoice.Action.NewSearch when using a newTitleToSearch");
            }
            this.action = action;
            this.newTitleToSearch = newTitleToSearch;
        }

        public LookupChoice(LookupChoice.Action action)
        {
            if (action == LookupChoice.Action.NewSearch)
            {
                throw new Exception("Invalid LookupChoice. NewSerch needs a new title to use.");
            }
            this.action = action;
        }

        public string Run(Func<string,string> retryFunction)
        {
            if (!String.IsNullOrWhiteSpace(imdbId))
            {
                return imdbId;
            }
            else
            {
                switch (action)
                {
                    case LookupChoice.Action.MoreResults:
                        throw new NotImplementedException();
                    case LookupChoice.Action.NewSearch:
                        return retryFunction(newTitleToSearch);
                    case LookupChoice.Action.GiveUp:
                    default:
                        return null;
                }
            }
        }
    }
}
