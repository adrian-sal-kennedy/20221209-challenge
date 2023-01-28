// true

namespace Ch1FlyoutPageModel.Models
{
    using System.Text;

    public record User : BaseModel
    {
        public string? FirstName { get; set; }

        // this would be better off as a List, for people like JRR Tolkien or GRR Martin...
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? NickName { get; set; }

        public string Initials
        {
            get
            {
                StringBuilder res = new();
                if (FirstName is { Length: > 0 } f) { res.Append(f[0]); }
                if (MiddleName is { Length: > 0 } m) { res.Append(m[0]); }
                if (LastName is { Length: > 0 } l) { res.Append(l[0]); }

                return res.ToString();
            }
        }

        public string FullName
        {
            get
            {
                if (NickName is { })
                {
                    return NickName;
                }

                if (this is { FirstName: { } f, LastName: { } l, })
                {
                    return $"{f} {l}";
                }

                return "Default User";
            }
        }
    }
}
