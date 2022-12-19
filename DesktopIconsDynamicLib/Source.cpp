#define UNICODE
#define _UNICODE
#define _CRT_SECURE_NO_WARNINGS
#include <windows.h>
#include <shlobj.h>
#include <exdisp.h>
#include <shlwapi.h>
#include <atlbase.h>
#include <atlalloc.h>
#include <stdio.h>
#include <iostream>
#include <vector>
using namespace std;

#define EXTERN_DLL_EXPORT extern "C" __declspec(dllexport)
class CCoInitialize {
public:
    CCoInitialize() : m_hr(CoInitialize(NULL)) { }
    ~CCoInitialize() { if (SUCCEEDED(m_hr)) CoUninitialize(); }
    operator HRESULT() const { return m_hr; }
    HRESULT m_hr;
};

void FindDesktopFolderView(REFIID riid, void** ppv)
{
    CComPtr<IShellWindows> spShellWindows;
    spShellWindows.CoCreateInstance(CLSID_ShellWindows);

    CComVariant vtLoc(CSIDL_DESKTOP);
    CComVariant vtEmpty;
    long lhwnd;
    CComPtr<IDispatch> spdisp;
    spShellWindows->FindWindowSW(
        &vtLoc, &vtEmpty,
        SWC_DESKTOP, &lhwnd, SWFO_NEEDDISPATCH, &spdisp);

    CComPtr<IShellBrowser> spBrowser;
    CComQIPtr<IServiceProvider>(spdisp)->
        QueryService(SID_STopLevelBrowser,
            IID_PPV_ARGS(&spBrowser));

    CComPtr<IShellView> spView;
    spBrowser->QueryActiveShellView(&spView);

    spView->QueryInterface(riid, ppv);
}

// CCoInitialize incorporated by reference
int __cdecl wmain(int argc, wchar_t** argv)
{
    CCoInitialize init;
    CComPtr<IFolderView2> spView;
    FindDesktopFolderView(IID_PPV_ARGS(&spView));
    CComPtr<IShellFolder> spFolder;
    spView->GetFolder(IID_PPV_ARGS(&spFolder));
    spView->SetCurrentFolderFlags(FWF_AUTOARRANGE | FWF_SNAPTOGRID, 0);

    CComPtr<IEnumIDList> spEnum;
    spView->Items(SVGIO_ALLVIEW, IID_PPV_ARGS(&spEnum));
    for (CComHeapPtr<ITEMID_CHILD> spidl;
        spEnum->Next(1, &spidl, nullptr) == S_OK;
        spidl.Free()) {
        STRRET str;
        spFolder->GetDisplayNameOf(spidl, SHGDN_NORMAL, &str);
        CComHeapPtr<wchar_t> spszName;
        StrRetToStr(&str, spidl, &spszName);

        POINT pt;
        spView->GetItemPosition(spidl, &pt);

        pt.x += (rand() % 50) - 2;
        pt.y += (rand() % 50) - 2;

        PCITEMID_CHILD apidl[1] = { spidl };
        spView->SelectAndPositionItems(
            1, apidl, &pt, SVSI_POSITIONITEM);

        wprintf(L"At %4d,%4d is %ls\n", pt.x, pt.y, spszName);
    }
    return 0;
}

EXTERN_DLL_EXPORT void** InitDesktop()
{
    CCoInitialize* init = new CCoInitialize();

    CComPtr<IFolderView2>* spView = new CComPtr<IFolderView2>();
    FindDesktopFolderView(IID_PPV_ARGS(&(*spView)));
    (*spView)->SetCurrentFolderFlags(FWF_AUTOARRANGE | FWF_SNAPTOGRID, 0); //Remove auto align flags

    CComPtr<IShellFolder>* spFolder = new CComPtr<IShellFolder>();
    spView->p->GetFolder(IID_PPV_ARGS(&(*spFolder)));
    
    void** ptrs = new void* [5] {init, spView->p, spFolder->p, spView, spFolder, };
    //cout << "Gave pointers: " << init  << "," << spView->p << "," << spFolder->p << spView << "," << spFolder << "," ;
    return ptrs;
}

EXTERN_DLL_EXPORT void Release(CCoInitialize* init, CComPtr<IFolderView2> folderPt, CComPtr<IShellFolder> shellPt)
{
    (folderPt.p)->SetCurrentFolderFlags(FWF_AUTOARRANGE | FWF_SNAPTOGRID, 1);

    delete init;
    folderPt.Release();
    shellPt.Release();
}

const wchar_t* GetWC(const char* c)
{
    const size_t cSize = strlen(c) + 1;
    wchar_t* wc = new wchar_t[cSize];
    mbstowcs(wc, c, cSize);

    return wc;
}

EXTERN_DLL_EXPORT void Free(void* ptr)
{
    delete ptr;
}

EXTERN_DLL_EXPORT POINT GetItemPosition(IFolderView* pView, IShellFolder* spFolder, const char* fName)
{
    const wchar_t* szName = GetWC(fName);
    CComPtr<IEnumIDList> spEnum;
    pView->Items(SVGIO_ALLVIEW, IID_PPV_ARGS(&spEnum));
    POINT pt;
    for (CComHeapPtr<ITEMID_CHILD> spidl;
        spEnum->Next(1, &spidl, nullptr) == S_OK;
        spidl.Free()) {
        STRRET str;
        spFolder->GetDisplayNameOf(spidl, SHGDN_NORMAL, &str);
        CComHeapPtr<wchar_t> spszName;
        StrRetToStr(&str, spidl, &spszName);
        if (!wcscmp(spszName, szName))
            pView->GetItemPosition(spidl, &pt);
    }
    delete szName;
    return pt;
}

EXTERN_DLL_EXPORT bool SetItemPosition(IFolderView* pView, IShellFolder* spFolder, const char* fName, POINT pt)
{
    const wchar_t* szName = GetWC(fName);
    CComPtr<IEnumIDList> spEnum;
    bool found = false;
    pView->Items(SVGIO_ALLVIEW, IID_PPV_ARGS(&spEnum));
    for (CComHeapPtr<ITEMID_CHILD> spidl;
        spEnum->Next(1, &spidl, nullptr) == S_OK;
        spidl.Free()) {
        STRRET str;
        spFolder->GetDisplayNameOf(spidl, SHGDN_NORMAL, &str);
        CComHeapPtr<wchar_t> spszName;
        StrRetToStr(&str, spidl, &spszName);

        if (!wcscmp(spszName, szName))
        {
            PCITEMID_CHILD apidl[1] = { spidl };
            HRESULT res = pView->SelectAndPositionItems(1, apidl, &pt, SVSI_POSITIONITEM);
            found = SUCCEEDED(res);
        }
    }
    delete szName;
    return found;
}

EXTERN_DLL_EXPORT POINT GetItemPositionById(IFolderView* pView, IShellFolder* spFolder, int index)
{
    CComHeapPtr<ITEMID_CHILD> item;
    pView->Item(index, &item);
    POINT pt;
    pView->GetItemPosition(item, &pt);
    return pt;
}

EXTERN_DLL_EXPORT bool SetItemPositionById(IFolderView* pView, IShellFolder* spFolder, int index, POINT pt)
{
    CComHeapPtr<ITEMID_CHILD> item;
    pView->Item(index, &item);
    PCITEMID_CHILD apidl[1] = { item };
    HRESULT res = pView->SelectAndPositionItems(1, apidl, &pt, SVSI_POSITIONITEM);
    return SUCCEEDED(res);
}

EXTERN_DLL_EXPORT bool SetItemsPositionById(IFolderView* pView, IShellFolder* spFolder, int* indexs, POINT* pt, int ct)
{
    PCITEMID_CHILD* apidl = new PCITEMID_CHILD[ct];
    for (size_t i = 0; i < ct; i++)
    {
        CComHeapPtr<ITEMID_CHILD> item;
        HRESULT res = pView->Item(indexs[i], &item);
        apidl[i] = item;
    }
    HRESULT res = pView->SelectAndPositionItems(ct, apidl, pt, SVSI_POSITIONITEM);
    delete[] apidl;
    return SUCCEEDED(res);
}

EXTERN_DLL_EXPORT int GetItemsCount(IFolderView* pView)
{
    int count = -1;
    pView->ItemCount(SVGIO_ALLVIEW, &count);
    return count;
}

EXTERN_DLL_EXPORT void GetItemId(int index, IFolderView* pView, IShellFolder* spFolder, char* dest)
{
    CComHeapPtr<ITEMID_CHILD> item;
    pView->Item(index, &item);
    STRRET str;
    spFolder->GetDisplayNameOf(item, SHGDN_NORMAL, &str);
    CComHeapPtr<wchar_t> spszName;
    StrRetToStr(&str, item, &spszName);
    sprintf(dest, "%ws", spszName.m_pData); //wchar_t to char*
}

EXTERN_DLL_EXPORT int GetIconsSize(IFolderView2* pView)
{
    int size = -1;
    FOLDERVIEWMODE viewMode = FVM_AUTO;
    pView->GetViewModeAndIconSize(&viewMode, &size);
    return size;
}

EXTERN_DLL_EXPORT bool SetIconsSize(IFolderView2* pView, int size)
{
    FOLDERVIEWMODE viewMode = FVM_AUTO;
    HRESULT res = pView->SetViewModeAndIconSize(viewMode, size);
    return SUCCEEDED(res);
}

EXTERN_DLL_EXPORT int GetSelectedIcon(IFolderView2* spView)
{
    int id = -1;
    spView->GetSelectedItem(0, &id);
    return id;
}