using SportEventsApp.Models;
using SportEventsApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SportEventsApp.Controllers.Admin
{
    [Authorize]
    public class InfoController : Controller
    {
        private ApplicationDbContext _context;
        public InfoController()
        {
            _context = new ApplicationDbContext();
        }
        #region simple
        // GET: Info
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var info = _context.Infos.FirstOrDefault(e => e.Id == id);
            if (info == null)
            {
                return HttpNotFound();
            }
            return View(info);
            
        }
        public ActionResult New()
        {
            var vm = new RegularInfoViewModel();
            var complex = _context.NestedInfos.Select(nn => new { nn.Id, nn.Header }).ToList();
            ViewBag.complex = complex;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(RegularInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("New", model);

            }
            if (model.Points.Count==0)
            {
                ModelState.AddModelError("Points", "You Have To Add At Least One Point ");
                return View("New", model);
            }

            if (model.Id == 0)
            {
                var info = new RegularInfo ()
                {
                    Title = model.Title,
                    NestedId = model.ComplexId
                };
                _context.Infos.Add(info);
                _context.SaveChanges();

                foreach (var item in model.Points)
                {
                    var point = new InfoPoint();
                    point.Value = item;
                    point.InfoId = info.Id;
                    _context.Points.Add(point);
                }

            }
            else
            {
                var dbInfo = _context.Infos.SingleOrDefault(i => i.Id == model.Id);
                dbInfo.Title = model.Title;
                dbInfo.NestedId = model.ComplexId;
                for (int i = 0; i < model.Points.Count; i++)
                {
                    var v = _context.Points.Where(vo => vo.InfoId == model.Id).OrderBy(vf => vf.Id).Skip(i).Take(1).FirstOrDefault();
                    if (v != null)
                    {
                        v.Value = model.Points[i];
                    }
                    else
                    {
                        var point = new InfoPoint();
                        point.Value = model.Points[i];
                       
                        point.InfoId = model.Id;
                        _context.Points.Add(point);
                    }
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Info");
        }

        public ActionResult Edit(int id)
        {
            var model = _context.Infos.SingleOrDefault(cu => cu.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var points = _context.Points.Where(v => v.InfoId == model.Id).Select(vc => vc.Value).ToList();

            var viewModel = new RegularInfoViewModel()
            {
                Id=model.Id,
                ComplexId = model.NestedId,
                Title = model.Title,
                Points = points
            };
            var complex = _context.NestedInfos.Select(nn => new { nn.Id, nn.Header }).ToList();
            ViewBag.complex = complex;
            return View("New", viewModel);
        }
        #endregion

        #region complex
        public ActionResult NewNested()
        {
            var model = new ComplexInfoViewModel();
            return View(model);
        }
        public ActionResult DetailsNested(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var info = _context.NestedInfos.FirstOrDefault(e => e.Id == id);
            if (info == null)
            {
                return HttpNotFound();
            }
            return View(info);
        }
        public ActionResult EditNested(int id)
        {
            var model = _context.NestedInfos.SingleOrDefault(ne => ne.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var simple = _context.Infos.Where(i => i.NestedId == model.Id).Select(r => new RegularInfoViewModel
            {
                Id = r.Id,
                Title = r.Title,
                Points = r.Points.Select(p => p.Value).ToList()
            }).ToList();

            var viewModel = new ComplexInfoViewModel()
            {
                Id = model.Id,
                Header = model.Header,
                RegularInfo = simple,
                Titles = simple.Select(tt => tt.Title).ToList(),
                RegularId = simple.Select(tt => tt.Id).ToList()
            };
            return View("NewNested",viewModel);
        }
        public ActionResult SaveNested(ComplexInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("NewNested", model);

            }
            if (model.RegularId.Count == 0)
            {
                ModelState.AddModelError("RegularId", "You Have To Add At Least One Simple Info ");
                return View("NewNested", model);
            }

            if (model.Id == 0)
            {

                var nested = new NestedInfo();
                nested.Header = model.Header;
                foreach (var item in model.RegularId)
                {
                    var simple = _context.Infos.SingleOrDefault(ss => ss.Id == item);
                    nested.InfoList.Add(simple);
                }
                _context.NestedInfos.Add(nested);
                _context.SaveChanges();

            }
            else
            {
                var dbNested = _context.NestedInfos.SingleOrDefault(i => i.Id == model.Id);
                dbNested.Header = model.Header;
                dbNested.InfoList.Clear();
                foreach (var item in model.RegularId)
                {
                    var simple = _context.Infos.SingleOrDefault(ss => ss.Id == item);
                    dbNested.InfoList.Add(simple);
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Info");
            
        }
        public ActionResult NewPartial()
        {
            var vm = new RegularInfoViewModel();
            //var complex = _context.NestedInfos.Select(nn => new { nn.Id, nn.Header }).ToList();
            //ViewBag.complex = complex;
            return PartialView("_SimpleInfo",vm);
        }


        public ActionResult SaveSimple(RegularInfoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return HttpNotFound("The Title field is required.");
            }
            if (model.Points.Count == 0)
            {
                ModelState.AddModelError("Points", "You Have To Add At Least One Point ");
                return HttpNotFound("You Have To Add At Least One Point");
            }

            if (model.Id == 0)
            {
                var info = new RegularInfo()
                {
                    Title = model.Title,
                    
                };
                _context.Infos.Add(info);
                _context.SaveChanges();

                foreach (var item in model.Points)
                {
                    if (String.IsNullOrWhiteSpace(item))
                    {
                        return HttpNotFound("You Have To Add At Least One Point");
                    }
                    var point = new InfoPoint();
                    point.Value = item;
                    point.InfoId = info.Id;
                    _context.Points.Add(point);
                }

                _context.SaveChanges();
                model.Id = info.Id;

            }
            else
            {
                var dbInfo = _context.Infos.SingleOrDefault(i => i.Id == model.Id);
                dbInfo.Title = model.Title;
                dbInfo.NestedId = model.ComplexId;
                for (int i = 0; i < model.Points.Count; i++)
                {
                    var v = _context.Points.Where(vo => vo.InfoId == model.Id).OrderBy(vf => vf.Id).Skip(i).Take(1).FirstOrDefault();
                    if (v != null)
                    {
                        v.Value = model.Points[i];
                    }
                    else
                    {
                        var point = new InfoPoint();
                        point.Value = model.Points[i];
                        point.InfoId = model.Id;
                        _context.Points.Add(point);
                    }
                }
                _context.SaveChanges();
                model.Id = dbInfo.Id;
            }



            return Json(new
            {
                id = model.Id,
                title = model.Title
            });
        }


        public ActionResult EditPartial(int id)
        {
            var model = _context.Infos.SingleOrDefault(cu => cu.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var points = _context.Points.Where(v => v.InfoId == model.Id).Select(vc => vc.Value).ToList();

            var viewModel = new RegularInfoViewModel()
            {
                Id  =model.Id,
                ComplexId = model.NestedId,
                Title = model.Title,
                Points = points
            };
            if (viewModel.ComplexId != null)
            {
                var complex = _context.NestedInfos.Select(nn => new { nn.Id, nn.Header }).ToList();
                ViewBag.complex = complex;
            }
            return PartialView("_SimpleInfo", viewModel);
        }
        #endregion


    }
}