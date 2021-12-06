namespace Rotomdex.Domain.Models
{
    public class Translation
    {
        private readonly string _text;

        public Translation(string text)
        {
            _text = text;
        }

        public override string ToString()
        {
            return _text;
        }
    }
}