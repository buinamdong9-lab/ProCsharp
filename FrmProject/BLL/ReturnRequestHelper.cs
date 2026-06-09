namespace FrmProject.BLL
{
    internal sealed class ReturnRequestItem
    {
        public int DeviceID { get; init; }
        public int InstanceID { get; init; }
        public int BorrowQty { get; init; }
        public int ReturnQty { get; init; }
        public string Note { get; init; } = string.Empty;
    }

    internal static class ReturnRequestHelper
    {
        private const string Prefix = "REQ|";
        private const int MaxPayloadLength = 255;
        private const int MaxNoteLengthPerItem = 40;

        public static string BuildPayload(DateTime requestedAt, IEnumerable<(int DeviceID, int InstanceID, int BorrowQty, int ReturnQty, string Note)> items)
        {
            List<string> encodedItems = items
                .Where(item => item.ReturnQty > 0)
                .Select(item =>
                {
                    string cleanNote = SanitizeNote(item.Note);
                    if (cleanNote.Length > MaxNoteLengthPerItem)
                        cleanNote = cleanNote.Substring(0, MaxNoteLengthPerItem);

                    return string.IsNullOrWhiteSpace(cleanNote)
                        ? $"{item.DeviceID}:{item.InstanceID}>{item.ReturnQty}"
                        : $"{item.DeviceID}:{item.InstanceID}>{item.ReturnQty}>{cleanNote}";
                })
                .ToList();

            string header = Prefix + new DateTimeOffset(requestedAt).ToUnixTimeSeconds() + "|";
            List<string> payloadItems = new List<string>();
            int currentLength = header.Length;

            foreach (string encodedItem in encodedItems)
            {
                int additionalLength = encodedItem.Length + (payloadItems.Count > 0 ? 1 : 0);
                if (currentLength + additionalLength > MaxPayloadLength)
                    break;

                payloadItems.Add(encodedItem);
                currentLength += additionalLength;
            }

            return header + string.Join("~", payloadItems);
        }

        public static bool TryParsePayload(string? payload, out DateTime requestedAt, out List<ReturnRequestItem> items)
        {
            requestedAt = DateTime.MinValue;
            items = new List<ReturnRequestItem>();

            if (string.IsNullOrWhiteSpace(payload) || !payload.StartsWith(Prefix, StringComparison.Ordinal))
                return false;

            string[] sections = payload.Split('|', 3);
            if (sections.Length < 3)
                return false;

            if (!TryParseRequestedAt(sections[1], out requestedAt))
                return false;

            foreach (string rawItem in sections[2].Split('~', StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = rawItem.Split('>', 4);
                if (parts.Length < 2)
                    continue;

                int deviceId = 0;
                int instanceId = 0;
                string[] idParts = parts[0].Split(':');
                if (idParts.Length == 2)
                {
                    int.TryParse(idParts[0], out deviceId);
                    int.TryParse(idParts[1], out instanceId);
                }
                else
                {
                    if (!int.TryParse(parts[0], out deviceId)) continue;
                }

                int borrowQty = 0;
                int returnQty;
                string note = string.Empty;

                if (parts.Length >= 3 && int.TryParse(parts[1], out int legacyBorrowQty) && int.TryParse(parts[2], out int legacyReturnQty))
                {
                    borrowQty = legacyBorrowQty;
                    returnQty = legacyReturnQty;
                    note = parts.Length == 4 ? parts[3] : string.Empty;
                }
                else if (int.TryParse(parts[1], out int compactReturnQty))
                {
                    returnQty = compactReturnQty;
                    note = parts.Length >= 3 ? parts[2] : string.Empty;
                }
                else
                {
                    continue;
                }

                items.Add(new ReturnRequestItem
                {
                    DeviceID = deviceId,
                    InstanceID = instanceId,
                    BorrowQty = borrowQty,
                    ReturnQty = returnQty,
                    Note = note
                });
            }

            return items.Count > 0;
        }

        private static string SanitizeNote(string? note) =>
            (note ?? string.Empty)
                .Replace("|", "/")
                .Replace("~", "/")
                .Replace(">", "/")
                .Trim();

        private static bool TryParseRequestedAt(string rawValue, out DateTime requestedAt)
        {
            if (long.TryParse(rawValue, out long unixSeconds))
            {
                requestedAt = DateTimeOffset.FromUnixTimeSeconds(unixSeconds).LocalDateTime;
                return true;
            }

            return DateTime.TryParse(rawValue, out requestedAt);
        }
    }
}

