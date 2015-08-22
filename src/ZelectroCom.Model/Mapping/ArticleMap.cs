using ZelectroCom.Data.Models;

namespace ZelectroCom.Data.Mapping
{
    public class ArticleMap : BaseMap<Article>  
    {
        public ArticleMap()
        {
            Property(x => x.AuthorId).IsRequired();
            Property(x => x.ArticleState).IsRequired();

            ToTable("Article");

            HasRequired(x => x.Author)
                .WithMany(y => y.Articles)
                .HasForeignKey(x => x.AuthorId)
                .WillCascadeOnDelete(false);

            HasOptional(x => x.Section).WithMany(y => y.Articles).HasForeignKey(x => x.SectionId).WillCascadeOnDelete(false);
        }
    }
}
