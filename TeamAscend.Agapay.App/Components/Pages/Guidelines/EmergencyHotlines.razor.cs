using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using TeamAscend.Agapay.App.Models;
using TeamAscend.Agapay.App.Shared;

namespace TeamAscend.Agapay.App.Components.Pages.Guidelines
{
    public partial class EmergencyHotlines : ComponentBase
    {
        private List<AppPhonebook> allContacts = new();
        private List<AppPhonebook> filteredContacts = new();
        private HashSet<string> favoriteContactIds = new();
        private string selectedFilter = "All";

        protected override void OnInitialized()
        {
            LoadContacts();
            filteredContacts = allContacts;
        }

        private void LoadContacts()
        {
            allContacts = DatabaseContext.Instance.PhonebookEntries
                .Where(contact => !contact.IsDeleted)
                .OrderBy(contact => contact.ContactName)
                .ToList();
        }

        private void ApplyFilterContacts(string filter)
        {
            selectedFilter = filter;
            
            if (filter == "All")
            {
                filteredContacts = allContacts;
            }
            else if (filter == "Favorites")
            {
                filteredContacts = allContacts
                    .Where(c => favoriteContactIds.Contains(c.ID.ToString()))
                    .ToList();
            }
            else if (filter == "Service / Agency")
            {
                filteredContacts = allContacts
                    .Where(c => !string.IsNullOrEmpty(c.Agency))
                    .ToList();
            }
            else if (filter.StartsWith("District"))
            {
                string districtNumber = filter.Split(' ')[1];
                filteredContacts = allContacts
                    .Where(c => c.District == districtNumber)
                    .ToList();
            }
        }

        private void ToggleFavorite(string contactId)
        {
            if (favoriteContactIds.Contains(contactId))
            {
                favoriteContactIds.Remove(contactId);
            }
            else
            {
                favoriteContactIds.Add(contactId);
            }

            if (selectedFilter == "Favorites")
            {
                ApplyFilterContacts("Favorites");
            }
        }

        private bool IsFavorite(string contactId)
        {
            return favoriteContactIds.Contains(contactId);
        }
    }
}
