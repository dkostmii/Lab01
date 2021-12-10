using System;


namespace Lab01.Analysis
{
    public class Article
    {
        public Place Place { get; private set; }
        public string Text { get; private set; }

        public Article(Place place, String text)
        {
            Place = place;
            Text = text;
        }

        public int Length { get { return Text.Length; } }


        // Static
        public static bool SelectArticle(ArticlesREUTERS article)
        {
            return article.PLACES.Length == 1 &&
                   Places.CheckTag(article.PLACES[0]) &&
                   article.TEXT.BODY != null;
        }

        public static Article CreateArticle(ArticlesREUTERS articleRaw)
        {
            return new Article(new Place(articleRaw.PLACES[0]), articleRaw.TEXT.BODY);
        }
    }
}
