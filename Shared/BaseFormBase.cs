using Microsoft.AspNetCore.Components;

namespace FormsMigrationApp.Shared
{
    // Equivalent of your Forms 6i subclassed template form.
    // Shared Save / Next Record / Execute Query behavior lives here.
    // Each concrete form (e.g. SetupQualityForm) inherits this and only
    // supplies the pieces that are unique to it via the abstract methods below.
    public abstract class BaseFormBase<TRecord> : ComponentBase where TRecord : class
    {
        protected List<TRecord> Records { get; set; } = new();
        protected int CurrentIndex { get; set; } = 0;
        protected TRecord? CurrentRecord => Records.Count > 0 && CurrentIndex < Records.Count
            ? Records[CurrentIndex]
            : null;

        protected string? StatusMessage { get; set; }
        protected bool IsBusy { get; set; }

        // Concrete form supplies: how to load records (its own "Execute Query" filter logic)
        protected abstract Task<List<TRecord>> LoadRecordsAsync();

        // Concrete form supplies: how to persist the current record
        protected abstract Task SaveRecordAsync(TRecord record);

        // Concrete form supplies: a blank record with sensible defaults
        // (equivalent to Forms 6i's "Insert Record" / F6 behavior)
        protected abstract TRecord CreateNewRecord();

        protected virtual async Task ExecuteQuery()
        {
            IsBusy = true;
            StatusMessage = null;
            try
            {
                Records = await LoadRecordsAsync();
                CurrentIndex = 0;
                StatusMessage = $"{Records.Count} record(s) found.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Query failed: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected virtual async Task Save()
        {
            if (CurrentRecord == null) return;

            IsBusy = true;
            StatusMessage = null;
            try
            {
                await SaveRecordAsync(CurrentRecord);
                StatusMessage = "Record saved.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Save failed: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected virtual Task NextRecord()
        {
            if (CurrentIndex < Records.Count - 1)
                CurrentIndex++;
            else
                StatusMessage = "Already at last record.";

            return Task.CompletedTask;
        }

        protected virtual Task PreviousRecord()
        {
            if (CurrentIndex > 0)
                CurrentIndex--;
            else
                StatusMessage = "Already at first record.";

            return Task.CompletedTask;
        }

        // Equivalent of Forms 6i "Insert Record" — adds a blank row and moves to it.
        // The user fills in the fields, then clicks Save, which will INSERT
        // since no matching record exists yet.
        protected virtual Task NewRecord()
        {
            var blank = CreateNewRecord();
            Records.Add(blank);
            CurrentIndex = Records.Count - 1;
            StatusMessage = "New record — fill in the fields and click Save.";

            return Task.CompletedTask;
        }
    }
}
