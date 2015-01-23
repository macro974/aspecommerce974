using System.Web.Optimization;

namespace MC3Shopper
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Utilisez la version de développement de Modernizr pour le développement et l'apprentissage. Puis, une fois
            // prêt pour la production, utilisez l'outil de génération (bluid) sur http://modernizr.com pour choisir uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/jquery.avgrund.min.js",
                "~/Scripts/additionnal.js",
                "~/Scripts/respond.js",
                "~/Scripts/StepProcess.js",
                "~/Scripts/StepProcessStyle.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/avgrund.css",
                "~/Content/bootstrap.css",
                "~/Content/additionnal.css",
                "~/Content/site.css"
                ));

            // Définissez EnableOptimizations sur False pour le débogage. Pour plus d'informations,
            // visitez http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}